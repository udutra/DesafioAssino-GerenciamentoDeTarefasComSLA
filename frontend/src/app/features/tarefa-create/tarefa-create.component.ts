import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TarefaService } from '../../core/services/tarefa.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-tarefa-create',
  standalone: false,
  templateUrl: './tarefa-create.component.html',
  styleUrls: ['./tarefa-create.component.scss']
})
export class TarefaCreateComponent {
  titulo: string = '';
  slaHoras: number = 24;
  arquivoSelecionado: File | null = null;
  enviando = false;

  constructor(
    private service: TarefaService,
    private router: Router,
    private snack: MatSnackBar
  ) {}

  onFileChange(event: any) {
    if (event.target.files.length > 0) {
      this.arquivoSelecionado = event.target.files[0];
    }
  }

  salvar() {
    if (!this.titulo || !this.arquivoSelecionado) {
      this.snack.open('Título e Arquivo são obrigatórios!', 'Ok', { duration: 3000 });
      return;
    }

    this.enviando = true;
    this.service.criar(this.titulo, this.slaHoras, this.arquivoSelecionado).subscribe({
      next: () => {
        this.snack.open('Tarefa criada com sucesso!', 'Ok', { duration: 3000 });
        this.router.navigate(['/tarefas']);
      },
      error: (err) => {
        console.error(err);
        this.snack.open('Erro ao criar tarefa. Verifique o backend.', 'Fechar');
        this.enviando = false;
      }
    });
  }
}
