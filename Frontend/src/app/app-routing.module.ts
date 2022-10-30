
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './pages/basic-pages/home/home.component';
import { LoginComponent } from './pages/basic-pages/login/login.component';
import { RegisterComponent } from './pages/basic-pages/register/register.component';
import { AboutBossComponent } from './pages/boss/about-boss/about-boss.component';
import { BossRankingComponent } from './pages/boss/boss-ranking/boss-ranking.component';
import { BossComponent } from './pages/boss/boss.component';
import { GuardService } from './services/auth/guard.service';



const routes: Routes = [
  {path:'',component:HomeComponent},
  {path:'login',component:LoginComponent},
  {path:'register',component:RegisterComponent},
  {path:'boss',component:BossComponent, canActivate: [GuardService], children:
  [
    {path: 'aboutBoss', component: AboutBossComponent},
    {path: 'bossRanking', component: BossRankingComponent},
  ]},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
