import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TarefaResponse, EnumStatus } from '../models/tarefa';

@Injectable({ providedIn: 'root' })
export class TarefaService {
  private apiUrl = 'https://localhost:7200/api/tarefas';

  constructor(private http: HttpClient) { }

  listar(status?: EnumStatus): Observable<TarefaResponse[]> {
    let url = this.apiUrl;
    if (status) {
      url += `?status=${status}`;
    }
    return this.http.get<TarefaResponse[]>(url);
  }

  criar(titulo: string, sla: number, arquivo: File): Observable<any> {
    const formData = new FormData();
    formData.append('Titulo', titulo);
    formData.append('SlaHoras', sla.toString());
    formData.append('Arquivo', arquivo);

    return this.http.post(this.apiUrl, formData);
  }

  concluir(numTarefa: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/${numTarefa}/concluir`, {});
  }
}
