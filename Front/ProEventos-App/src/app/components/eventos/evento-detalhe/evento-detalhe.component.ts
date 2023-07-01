import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  form!: FormGroup;

  constructor() { }

  ngOnInit(): void {
    this.validation();
  }

  public validation(): void {
    this.form = new FormGroup({
      local: new FormControl(),
      dataEvento: new FormControl(),
      tema: new FormControl(),
      quantidadePessoas: new FormControl(),
      imagemURL: new FormControl(),
      telefone: new FormControl(),
      email: new FormControl(),
    });
  }

}
