import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TarefaListComponent } from './features/tarefa-list/tarefa-list.component';
import { TarefaCreateComponent } from './features/tarefa-create/tarefa-create.component';

const routes: Routes = [
  { path: '', redirectTo: 'tarefas', pathMatch: 'full' },
  { path: 'tarefas', component: TarefaListComponent },
  { path: 'nova', component: TarefaCreateComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
