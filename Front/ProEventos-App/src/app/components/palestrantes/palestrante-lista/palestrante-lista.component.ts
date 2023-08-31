import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Palestrante } from '@app/models/Palestrante';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { PalestranteService } from '@app/services/palestrante.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-palestrante-lista',
  templateUrl: './palestrante-lista.component.html',
  styleUrls: ['./palestrante-lista.component.scss']
})
export class PalestranteListaComponent implements OnInit {

  public palestrantes: Palestrante[] = [];
  public pagination = {} as Pagination;
  termoBuscaChanged: Subject<string> = new Subject<string>();
  public palestranteId = 0;

  constructor(
    private palestranteService: PalestranteService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router,) { }

  ngOnInit(): void {
    this.pagination = { currentPage: 1, itemsPerPage: 3, totalItems: 1 } as Pagination;
    this.carregarPalestrantes();
  }

  public filtrarPalestrantes(palestrante: any): void {

    if (this.termoBuscaChanged.observers.length == 0) {
      this.termoBuscaChanged.pipe(debounceTime(1000)).subscribe(filtrarPor => {
        this.spinner.show();

        this.palestranteService.getPalestrantes(this.pagination.currentPage, this.pagination.itemsPerPage, palestrante.value).subscribe(
          (paginatedResult: PaginatedResult<Palestrante[]>) => {
            this.palestrantes = paginatedResult.result;
            this.pagination = paginatedResult.pagination;
          },
          (error) => {
            console.log(error);
            this.spinner.hide();
            this.toastr.error('Erro ao carregar os Palestrantes', 'Erro!');
          },
        ).add(() => this.spinner.hide());
      })
    }

    this.termoBuscaChanged.next(palestrante.value);
  }

  public carregarPalestrantes() {
    this.spinner.show();
    this.palestranteService.getPalestrantes(this.pagination.currentPage, this.pagination.itemsPerPage).subscribe(
      (paginatedResult: PaginatedResult<Palestrante[]>) => {
        this.palestrantes = paginatedResult.result;
        this.pagination = paginatedResult.pagination;
      },
      (error) => {
        console.log(error);
        this.spinner.hide();
        this.toastr.error('Erro ao carregar os Palestrantes', 'Erro!');
      },
    ).add(() => this.spinner.hide());
  }

  public getImagemURL(imagemName: string): string {
    if (imagemName != null && imagemName != '') {
      return environment.apiURL + 'Resources/Images/User/' + imagemName;
    }
    else { return 'assets/upload.png'; }
  }

}
