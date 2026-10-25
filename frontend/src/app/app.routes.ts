import { Routes } from '@angular/router';
import { MenuList } from './components/menu/menu-list/menu-list';
import { Pedidos } from './components/pedidos/pedidos';
import { Mesas } from './components/mesas/mesas';
import { Dashboard } from './components/dashboard/dashboard';

export const routes: Routes = [
  { path: '', redirectTo: 'menu', pathMatch: 'full' },
  { path: 'menu', component: MenuList },
  { path: 'pedidos', component: Pedidos },
  { path: 'mesas', component: Mesas },
  { path: 'dashboard', component: Dashboard },
  { path: '**', redirectTo: 'menu' },
];