import { Component, OnInit } from '@angular/core';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { environment } from '@environments/environment';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {
  public imagemURL = 'assets/upload.png';
  public file?: File;

  public usuario = {} as UserUpdate;
  public get Palestrante(): boolean {
    return this.usuario.funcao === 'Palestrante'
  }

  constructor(private spinner: NgxSpinnerService, private toastr: ToastrService, private accountService: AccountService) { }

  ngOnInit(): void {
  }

  public setFormValue(usuario: UserUpdate): void {
    this.usuario = usuario;
    if (usuario.imagemURL != null && usuario.imagemURL != '') {
      this.imagemURL = environment.apiURL + 'Resources/Images/User/' + usuario.imagemURL;
    }
  }

  onFileChange(ev: any): void {
    const reader = new FileReader();
    reader.onload = (event: any) => this.imagemURL = event.target.result;
    this.file = ev.target.files;
    reader.readAsDataURL(this.file![0]);
    this.uploadImagem();
  }

  private uploadImagem(): void {
    this.spinner.show();
    this.accountService.postUpload(this.file!).subscribe(
      () => this.toastr.success('Imagem atualizada com sucesso', 'Sucesso'),
      (error: any) => {
        this.toastr.error('Erro ao fazer upload de imagem', 'Erro');
        console.log(error);
      }).add(() => this.spinner.hide());
  }
}
