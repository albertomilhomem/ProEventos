import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { RedeSocial } from '@app/models/RedeSocial';
import { RedeSocialService } from '@app/services/rede-social.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-redes-sociais',
  templateUrl: './redes-sociais.component.html',
  styleUrls: ['./redes-sociais.component.scss']
})
export class RedesSociaisComponent implements OnInit {
  modalRef?: BsModalRef;
  @Input() eventoId = 0;
  public formRS!: FormGroup;
  public redeSocialAtual = { id: 0, nome: '', indice: 0 };

  constructor(private fb: FormBuilder, private modalService: BsModalService, private toastr: ToastrService, private spinner: NgxSpinnerService, private redeSocialService: RedeSocialService) { }

  ngOnInit(): void {
    this.carregarRedesSociais(this.eventoId);
    this.validation();
  }

  get redesSociais(): FormArray {
    return this.formRS.get('redesSociais') as FormArray;
  }

  private carregarRedesSociais(id: number = 0): void {
    this.spinner.show();

    let origem = 'palestrante';
    if (this.eventoId != 0) origem = 'evento';

    this.redeSocialService.getRedesSociais(origem, id).subscribe(
      (redeSocialRetorno: RedeSocial[]) => {
        redeSocialRetorno.forEach((redeSocial) => {
          this.redesSociais.push(this.criarRedeSocial(redeSocial))
        });
      },
      (error: any) => {
        this.toastr.error('Erro ao tentar carregar as redes sociais', 'Erro');
        console.log(error.error);
      }).add(() => this.spinner.hide())

  }

  public validation(): void {
    this.formRS = this.fb.group({
      redesSociais: this.fb.array([])
    });
  }

  adicionarRedeSocial(): void {
    this.redesSociais.push(this.criarRedeSocial({ id: 0 } as RedeSocial)
    );
  }

  criarRedeSocial(redeSocial: RedeSocial): FormGroup {
    return this.fb.group(
      {
        id: [redeSocial.id],
        nome: [redeSocial.nome, Validators.required],
        url: [redeSocial.url, Validators.required],
      }
    )
  }

  public retornaTitulo(nome: string): string {
    return nome == null || nome == '' ? 'Rede Social' : nome;
  }

  public cssValidator(campo: FormControl | AbstractControl): any {
    return { 'is-invalid': campo!.errors && campo!.touched }
  }

  public removerRedeSocial(template: TemplateRef<any>, indice: number): void {
    this.redeSocialAtual.id = this.redesSociais.get(indice + '.id')?.value;
    this.redeSocialAtual.nome = this.redesSociais.get(indice + '.nome')?.value;
    this.redeSocialAtual.indice = indice;

    this.modalRef = this.modalService.show(template, { class: 'modal-sm' })
  }

  confirmDeleteRedeSocial(): void {
    let origem = 'palestrante';
    if (this.eventoId != 0) origem = 'evento';

    this.modalRef?.hide();

    this.spinner.show();

    setTimeout(() => {
      this.redeSocialService.deleteRedeSocial(origem, this.eventoId, this.redeSocialAtual.id).subscribe(
        (result: any) => {
          if (result.message == 'Deletado') {
            this.toastr.success(`RedeSocial ${this.redeSocialAtual.nome} deletado com sucesso.`, 'Deletado!');
            this.redesSociais.removeAt(this.redeSocialAtual.indice);
          }
        },
        (error: any) => {
          console.log(error);
          this.toastr.error(`Erro ao tentar deletar o RedeSocial: ${this.redeSocialAtual.nome}`, 'Erro!');
        },
      ).add(() => this.spinner.hide());
    }, 2000);
  }

  declineDeleteRedeSocial(): void {
    this.modalRef?.hide();
  }

  public salvarRedesSociais(): void {
    let origem = 'palestrante';

    if (this.eventoId != 0) origem = 'evento';

    if (this.formRS.controls.redesSociais.valid) {
      this.spinner.show();
      this.redeSocialService.saveRedesSociais(origem, this.eventoId, this.formRS.value.redesSociais).subscribe(
        () => {
          this.toastr.success('Redes Sociais salvas com Sucesso!', 'Sucesso');
        },
        (error: any) => {
          this.toastr.error('Erro ao salvar as redes sociais', 'Erro');
          console.error(error);
        },
      ).add(() => this.spinner.hide())
    }
  }

}
