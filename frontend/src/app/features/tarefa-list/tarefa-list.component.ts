import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { TarefaResponse, EnumStatus } from '../../core/models/tarefa';
import { TarefaService } from '../../core/services/tarefa.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-tarefa-list',
  standalone: false,
  templateUrl: './tarefa-list.component.html',
  styleUrls: ['./tarefa-list.component.scss']
})
export class TarefaListComponent implements OnInit {
  displayedColumns: string[] = ['num', 'titulo', 'criacao', 'sla', 'expiracao', 'validade', 'status', 'acoes'];
  dataSource: TarefaResponse[] = [];

  constructor(
    private service: TarefaService,
    private snack: MatSnackBar,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.carregar();
  }

  carregar() {
    this.service.listar().subscribe({
      next: (dados) => {
        this.dataSource = dados;
        this.cdr.detectChanges();
      },
      error: (e) => console.error(e)
    });
  }

  concluir(tarefa: TarefaResponse) {
    if(confirm(`Deseja concluir a tarefa "${tarefa.titulo}"?`)) {
      this.service.concluir(tarefa.numTarefa).subscribe({
        next: () => {
          this.snack.open('Tarefa concluída!', 'OK', { duration: 3000 });
          this.carregar();
        },
        error: (err) => {
          this.snack.open('Erro ao concluir. Verifique se não está expirada.', 'X', { duration: 3000 });
        }
      });
    }
  }

  isExpirada(t: TarefaResponse): boolean {
    const status = t.status.toString();

    if (status === 'Concluida' || status === '2') return false;

    if (status === 'Expirada' || status === '3') return true;

    const criacao = new Date(t.dataCriacao);
    const limite = new Date(criacao.getTime() + (t.slaHoras * 60 * 60 * 1000))
    return new Date() > limite;
  }

  getStatusLabel(status: any): string {
    const s = status.toString();

    if (s === '1' || s === 'Pendente') return 'Pendente';
    if (s === '2' || s === 'Concluida') return 'Concluída';
    if (s === '3' || s === 'Expirada') return 'Expirada';

    return status;
  }

  getDate(dateStr: string): Date {
    return new Date(dateStr);
  }

  calcularSla(t: TarefaResponse): string {
    if (!t.dataCriacao || !t.dataExpiracao)
      return '--';

    const inicio = new Date(t.dataCriacao).getTime();
    const fim = new Date(t.dataExpiracao).getTime();

    const diff = fim - inicio;

    const horas = Math.round(diff / (1000 * 60 * 60));

    return horas.toString();
  }
}
