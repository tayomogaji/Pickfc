import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { SignupComponent } from './auth/signup/signup.component';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './auth/auth.guard';
import { GameComponent } from './game/game.component';
import { AdminComponent } from './admin/admin.component';
import { GamesComponent } from './games/games.component';
import { JoinComponent } from './game/join/join.component';
import { AddEditGameComponent } from './game/addedit-game/addedit-game.component';
import { GameSettingsComponent } from './game/game-settings/game-settings.component';
import { RulesPageComponent } from './rules/rules-page/rules-page.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'rules', component: RulesPageComponent },
  { path: 'sign-up', component: SignupComponent },
  { path: 'admin', component: AdminComponent, canActivate: [AuthGuard] },
  { path: 'my-games', component: GamesComponent, canActivate: [AuthGuard] },
  { path: 'create', component: AddEditGameComponent, canActivate: [AuthGuard] },
  { path: 'join', component: JoinComponent, canActivate: [AuthGuard] },
  {
    path: 'game/:id', component: GameComponent, canActivate: [AuthGuard], children: [
      { path: 'settings', component: GameSettingsComponent, canActivate: [AuthGuard] }
    ]
  },
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }
