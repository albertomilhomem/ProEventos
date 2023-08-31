import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { PalestranteService } from '@app/services/palestrante.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil-detalhe',
  templateUrl: './perfil-detalhe.component.html',
  styleUrls: ['./perfil-detalhe.component.scss']
})
export class PerfilDetalheComponent implements OnInit {
  @Output() changeFormValue = new EventEmitter();
  userUpdate = {} as UserUpdate;
  form!: FormGroup;

  get f(): any {
    return this.form.controls;
  }

  constructor(
    private fb: FormBuilder,
    public accountService: AccountService,
    public palestranteService: PalestranteService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService) { }

  ngOnInit(): void {
    this.validation();
    this.carregarUsuario();
    this.verificaForm();
  }

  public validation(): void {
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmePassword')
    }

    this.form = this.fb.group(
      {
        userName: [''],
        imagemURL: [''],
        titulo: ['NaoInformado', Validators.required],
        primeiroNome: ['', Validators.required],
        ultimoNome: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        phoneNumber: ['', Validators.required],
        funcao: ['NaoInformado', Validators.required],
        descricao: ['', Validators.required],
        password: ['', [Validators.minLength(4), Validators.nullValidator]],
        confirmePassword: ['', Validators.required]
      },


      formOptions
    )
  }

  onSubmit(): void {
    this.atualizarUsuario();
  }

  public atualizarUsuario() {
    this.userUpdate = { ... this.form.value };
    this.spinner.show();

    if (this.f.funcao.value == 'Palestrante') {
      this.palestranteService.post().subscribe(
        () => this.toastr.success("Função palestrante ativada.", "Sucesso"),
        (error: any) => {
          this.toastr.error("Erro ao ativar função Palestrante", "Error");
          console.error(error);
        }
      );
    }

    this.accountService.updateUser(this.userUpdate).subscribe(
      () => { this.toastr.success("Usuário atualizado com sucesso", "Sucesso") },
      (error) => {
        this.toastr.error(error.error);
        console.error(error);
      }
    ).add(() => this.spinner.hide())
  }

  public resetForm(event: any): void {
    event.preventDefault();
    this.form.reset();
  }

  private carregarUsuario(): void {
    this.spinner.show();
    this.accountService.getUser().subscribe(
      (userRetorno: UserUpdate) => {
        console.log(userRetorno);
        this.userUpdate = userRetorno;
        this.form.patchValue(this.userUpdate);
        this.toastr.success("Usuário carregado", "Sucesso")
      },
      (error) => {
        console.error(error);
        this.toastr.error("Erro ao carregar o usuário", "Erro");
        this.router.navigate(['/dashboard']);
      }
    ).add(() => this.spinner.hide());
  }

  private verificaForm(): void {
    this.form.valueChanges.subscribe(() => this.changeFormValue.emit({ ... this.form.value }))
  }
}
