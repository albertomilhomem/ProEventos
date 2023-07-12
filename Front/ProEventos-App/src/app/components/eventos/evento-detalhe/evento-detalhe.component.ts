import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { Lote } from '@app/models/Lote';
import { EventoService } from '@app/services/evento.service';
import { LoteService } from '@app/services/lote.service';
import { environment } from '@environments/environment';
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {
  eventoId: number = 0;
  datePickerConfig: Partial<BsDatepickerConfig>;
  evento = {} as Evento;
  form!: FormGroup;
  estadoSalvar: string = 'post';
  modalRef?: BsModalRef;
  loteAtual = { id: 0, nome: '', indice: 0 }
  imagemURL = 'assets/upload.png';
  file?: File;

  get f(): any {
    return this.form.controls;
  }

  get bsConfig(): any {
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY HH:mm',
      containerClass: 'theme-default',
      showWeekNumbers: false,
    }
  }

  get bsConfigLote(): any {
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY',
      containerClass: 'theme-default',
      showWeekNumbers: false,
    }
  }

  get modoEditar(): boolean {
    return this.estadoSalvar == 'put';
  }

  get lotes(): FormArray {
    return this.form.get('lotes') as FormArray;
  }

  constructor(
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private activatedRouter: ActivatedRoute,
    private eventoService: EventoService,
    private loteService: LoteService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private router: Router,
    private modalService: BsModalService) {
    this.localeService.use("pt-br");
    this.datePickerConfig = Object.assign({}, {
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY',
      containerClass: 'theme-dark-blue',
      showWeekNumbers: false
    });
  }

  ngOnInit(): void {
    this.spinner.show();
    this.validation();

    setTimeout(() => {
      this.carregarEvento();
    }, 2000)
  }

  public carregarEvento(): void {
    this.eventoId = +this.activatedRouter.snapshot.paramMap.get('id')!;


    if (this.eventoId != null && this.eventoId != 0) {
      this.estadoSalvar = 'put';
      this.eventoService.getEventosById(this.eventoId).subscribe(
        {
          next: (evento: Evento) => {
            this.evento = { ...evento }
            this.form.patchValue(this.evento);
            if (this.evento.imagemURL != null && this.evento.imagemURL != '') {
              this.imagemURL = environment.apiURL + 'Resources/Images/' + this.evento.imagemURL;
            }
            this.evento.lotes.forEach(lote => { this.lotes.push(this.criarLote(lote)); });
          },
          error: (error: any) => {
            console.error(error);
            this.spinner.hide();
            this.toastr.error('Erro ao carregar o Evento', 'Erro!')
          },
          complete: () => { this.spinner.hide(); },
        }
      )
    }
    else {
      this.spinner.hide();
    }
  }

  public validation(): void {
    this.form = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      quantidadePessoas: ['', [Validators.required, Validators.max(120000)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      lotes: this.fb.array([])
    });
  }

  adicionarLote(): void {
    (this.lotes).push(this.criarLote({ id: 0 } as Lote)
    );
  }

  criarLote(lote: Lote): FormGroup {
    return this.fb.group(
      {
        id: [lote.id],
        nome: [lote.nome, Validators.required],
        preco: [lote.preco, Validators.required],
        quantidade: [lote.quantidade, Validators.required],
        dataInicio: [lote.dataInicio, Validators.required],
        dataFim: [lote.dataFim, Validators.required],
      }
    )
  }

  public mudarValorData(value: Date, indice: number, campo: string) {
    this.lotes.value[indice][campo] = value;
  }

  public retornaTituloLote(nome: string): string {
    return nome == null || nome == '' ? 'Nome do Lote' : nome;
  }


  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(campo: FormControl | AbstractControl): any {
    return { 'is-invalid': campo!.errors && campo!.touched }
  }

  public salvarEvento(): void {
    this.spinner.show();

    if (this.form.valid) {
      this.evento = this.estadoSalvar === 'post' ? this.evento = { ... this.form.value } : { id: this.evento.id, ... this.form.value };

      this.eventoService[this.estadoSalvar](this.evento).subscribe(
        (eventoRetorno: Evento) => {
          this.toastr.success('Evento salvo com Sucesso!', 'Sucesso');
          this.router.navigate([`eventos/detalhe/${eventoRetorno.id}`]);
        },
        (error: any) => {
          console.error(error);
          this.spinner.hide();
          this.toastr.error('Erro ao salvar o evento', 'Erro');
        },
        () => { this.spinner.hide(); }
      );

    }

  }

  public salvarLotes(): void {

    if (this.form.controls.lotes.valid) {
      this.spinner.show();
      this.loteService.saveLote(this.eventoId, this.form.value.lotes).subscribe(
        () => {
          this.toastr.success('Lotes salvo com Sucesso!', 'Sucesso');
          //this.lotes.reset();
          //this.carregarLotes();
        },
        (error: any) => {
          console.error(error);
          this.spinner.hide();
          this.toastr.error('Erro ao salvar os lotes', 'Erro');
        },
      ).add(() => this.spinner.hide())
    }
  }

  public carregarLotes(): void {
    this.loteService.getLotesByEvento(this.eventoId).subscribe(
      (lotesRetorno: Lote[]) => {
        lotesRetorno.forEach(lote => { this.lotes.push(this.criarLote(lote)); });
      },
      (error: any) => { this.toastr.error('Erro ao tentar carregar lotes', 'Erro'); console.log(error); }
    ).add(() => this.spinner.hide)
  }

  public removerLote(template: TemplateRef<any>, indice: number): void {
    this.loteAtual.id = this.lotes.get(indice + '.id')?.value;
    this.loteAtual.nome = this.lotes.get(indice + '.nome')?.value;
    this.loteAtual.indice = indice;

    this.modalRef = this.modalService.show(template, { class: 'modal-sm' })
  }

  confirmDeleteLote(): void {
    this.modalRef?.hide();

    this.spinner.show();

    setTimeout(() => {
      this.loteService.deleteLote(this.eventoId, this.loteAtual.id).subscribe(
        (result: any) => {
          if (result.message == 'Deletado') {
            this.toastr.success(`Lote ${this.loteAtual.nome} deletado com sucesso.`, 'Deletado!');
            this.lotes.removeAt(this.loteAtual.indice);
          }
        },
        (error: any) => {
          console.log(error);
          this.toastr.error(`Erro ao tentar deletar o Lote: ${this.loteAtual.nome}`, 'Erro!');
        },
      ).add(() => this.spinner.hide());
    }, 2000);


  }
  declineDeleteLote(): void {
    this.modalRef?.hide();
  }

  onFileChange(ev: any): void {
    const reader = new FileReader();

    reader.onload = (event: any) => this.imagemURL = event.target.result;

    this.file = ev.target.files;

    reader.readAsDataURL(this.file![0]);

    this.uploadImagem();
  }

  uploadImagem(): void {
    this.spinner.show();
    this.eventoService.postUpload(this.eventoId, this.file!).subscribe(
      () => { this.carregarEvento(); this.toastr.success("Imagem atualizada com sucesso!", "Sucesso!") },
      (error: any) => { this.toastr.error("Erro ao tentar atualizada a imagem!", "Erro!"); console.log(error) }
    ).add(() => this.spinner.hide());
  }

}
