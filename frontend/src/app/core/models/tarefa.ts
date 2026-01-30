export enum EnumStatus {
  Pendente = 1,
  Concluida = 2,
  Expirada = 3
}

export interface TarefaResponse {
  id: string;
  numTarefa: number;
  titulo: string;
  slaHoras: number;
  dataCriacao: string;
  dataConclusao?: string;
  dataExpiracao: string;
  arquivoPath: string;
  status: EnumStatus;
}
