import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Palestrante } from '@app/models/Palestrante';
import { PalestranteService } from '@app/services/palestrante.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { debounceTime, map, tap } from 'rxjs/operators';

@Component({
  selector: 'app-palestrante-detalhe',
  templateUrl: './palestrante-detalhe.component.html',
  styleUrls: ['./palestrante-detalhe.component.scss']
})
export class PalestranteDetalheComponent implements OnInit {
  public form!: FormGroup;
  public situacaoForm = '';
  public corDescricao = '';

  public get f(): any {
    return this.form.controls;
  }

  constructor(private fb: FormBuilder, public palestranteService: PalestranteService, private toastr: ToastrService, private spinner: NgxSpinnerService) { }

  ngOnInit(): void {
    this.validation();
    this.verificaForm();
    this.carregarPalestrante();
  }

  private validation(): void {
    this.form = this.fb.group(
      {
        miniCurriculo: ['']
      }
    )
  }

  private carregarPalestrante(): void {
    this.spinner.show();

    this.palestranteService.getPalestrante().subscribe(
      (palestrante: Palestrante) => {
        this.form.patchValue(palestrante);
      },
      (error: any) => {
        this.toastr.error('Erro ao carregar o palestrante', 'Erro');
        console.log(error.error)
      });
  }

  private verificaForm(): void {
    this.form.valueChanges.pipe(
      map(() => {
        this.situacaoForm = 'Minicurrículo está sendo atualizado!';
        this.corDescricao = 'text-warning'
      }),
      debounceTime(1000),
      tap(() => this.spinner.show())
    ).subscribe(
      () => {
        this.palestranteService.put({ ... this.form.value }).subscribe(
          () => {
            this.situacaoForm = 'Minicurriculo foi atualizado';
            this.corDescricao = 'text-success'

            setTimeout(() => {
              this.situacaoForm = 'Minicurriculo foi carregado';
              this.corDescricao = 'text-muted'
            }, 2000)
          },
          (error: any) => { this.toastr.error('Erro ao tentar atualizar Palestrante', 'Erro'); console.log(error.error); }
        ).add(() => this.spinner.hide())
      });
  }
}
