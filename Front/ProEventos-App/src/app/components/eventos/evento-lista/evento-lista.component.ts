import { Component, OnInit, TemplateRef } from '@angular/core';

import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinner, NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { EventoService } from '@app/services/evento.service';

import { Evento } from '@app/models/Evento';
import { Router } from '@angular/router';
import { environment } from '@environments/environment';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {
  public eventos: Evento[] = [];
  modalRef?: BsModalRef;
  public eventoId = 0;
  public pagination = {} as Pagination;

  public larguraImagem: number = 150;
  public margemImagem: number = 2;
  public isCollapsed = false;

  termoBuscaChanged: Subject<string> = new Subject<string>();

  public filtrarEventos(evento: any): void {

    if (this.termoBuscaChanged.observers.length == 0) {
      this.termoBuscaChanged.pipe(debounceTime(1000)).subscribe(filtrarPor => {
        this.spinner.show();

        this.eventoService.getEventos(this.pagination.currentPage, this.pagination.itemsPerPage, evento.value).subscribe(
          (paginatedResult: PaginatedResult<Evento[]>) => {
            this.eventos = paginatedResult.result;
            this.pagination = paginatedResult.pagination;
          },
          (error) => {
            console.log(error);
            this.spinner.hide();
            this.toastr.error('Erro ao carregar os Eventos', 'Erro!');
          },
        ).add(() => this.spinner.hide());
      })
    }

    this.termoBuscaChanged.next(evento.value);
  }


  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router,
  ) { }

  public ngOnInit(): void {
    this.pagination = { currentPage: 1, itemsPerPage: 3, totalItems: 1 } as Pagination;
    this.carregarEventos();

  }

  public carregarEventos() {
    this.spinner.show();
    this.eventoService.getEventos(this.pagination.currentPage, this.pagination.itemsPerPage).subscribe(
      (paginatedResult: PaginatedResult<Evento[]>) => {
        this.eventos = paginatedResult.result;
        this.pagination = paginatedResult.pagination;
      },
      (error) => {
        console.log(error);
        this.spinner.hide();
        this.toastr.error('Erro ao carregar os Eventos', 'Erro!');
      },
    ).add(() => this.spinner.hide());
  }

  public alterarImagem(): void {
    this.isCollapsed = !this.isCollapsed;
  }

  openModal(event: any, template: TemplateRef<any>, eventoId: number) {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  public pageChanged(event): void {
    this.pagination.currentPage = event.page;
    this.carregarEventos();
  }

  confirm(): void {
    this.modalRef?.hide();

    setTimeout(() => {
      this.eventoService.deleteEvento(this.eventoId).subscribe(
        (result: any) => {
          if (result.message == 'Deletado') {
            this.toastr.success('Evento deletado com sucesso.', 'Deletado!');
            this.carregarEventos();
          }
        },
        (error: any) => {
          console.log(error);
          this.toastr.error(`Erro ao tentar deletar o Evento: ${this.eventoId}`, 'Erro!');
        },
      ).add(() => this.spinner.hide());
    }, 2000);


  }
  decline(): void {
    this.modalRef?.hide();
  }

  detalheEvento(id: number): void {
    this.router.navigate([`eventos/detalhe/${id}`]);
  }

  public retornaImagem(imagemURL: string, eventoId: number): string {
    return (imagemURL != null && imagemURL != '') ? `${environment.apiURL}Resources/Images/${imagemURL}` : 'http://lorempixel.com.br/50/50/' + eventoId;
  }

}
