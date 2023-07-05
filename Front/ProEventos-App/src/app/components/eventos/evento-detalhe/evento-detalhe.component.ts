import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  evento = {} as Evento;
  form!: FormGroup;
  estadoSalvar: string = 'post';

  get f(): any {
    return this.form.controls;
  }

  get bsConfig(): any {
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm',
      containerClass: 'theme-default',
      showWeekNumbers: false,
    }
  }

  constructor(
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private router: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService) {
    this.localeService.use("pt-br");
  }

  ngOnInit(): void {
    this.spinner.show();
    this.validation();

    setTimeout(() => {
      this.carregarEvento();
    }, 2000)
  }

  public carregarEvento(): void {
    const eventoIdParam = this.router.snapshot.paramMap.get('id');


    if (eventoIdParam != null) {
      this.estadoSalvar = 'put';
      this.eventoService.getEventosById(+eventoIdParam).subscribe(
        {
          next: (evento: Evento) => {
            this.evento = { ...evento }
            this.form.patchValue(this.evento);
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
      imagemURL: ['', Validators.required],
    });
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(campo: FormControl): any {
    return { 'is-invalid': campo.errors && campo.touched }
  }

  public salvarAlteracao(): void {
    this.spinner.show();

    if (this.form.valid) {
      this.evento = this.estadoSalvar === 'post' ? this.evento = { ... this.form.value } : { id: this.evento.id, ... this.form.value };

      this.eventoService[this.estadoSalvar](this.evento).subscribe(
        () => { this.toastr.success('Evento salvo com Sucesso!', 'Sucesso') },
        (error: any) => {
          console.error(error);
          this.spinner.hide();
          this.toastr.error('Erro ao salvar o evento', 'Erro');
        },
        () => { this.spinner.hide(); }
      );

    }

  }

}
