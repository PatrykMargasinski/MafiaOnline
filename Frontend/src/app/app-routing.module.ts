import { SendMessageComponent } from './pages/message/send-message/send-message.component';
import { ShowReportsComponent } from './pages/message/show-reports/show-reports.component';
import { ShowMessagesComponent } from './pages/message/show-messages/show-messages.component';

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AgentComponent } from './pages/agent/agent.component';
import { MissionComponent } from './pages/mission/mission.component';
import { AgentsForSaleComponent } from './pages/agent/agents-for-sale/agents-for-sale.component';
import { AgentsOnMissionComponent } from './pages/agent/agents-on-mission/agents-on-mission.component';
import { AvailableAgentsComponent } from './pages/agent/available-agents/available-agents.component';
import { HomeComponent } from './pages/basic-pages/home/home.component';
import { LoginComponent } from './pages/basic-pages/login/login.component';
import { RegisterComponent } from './pages/basic-pages/register/register.component';
import { AboutBossComponent } from './pages/boss/about-boss/about-boss.component';
import { BossRankingComponent } from './pages/boss/boss-ranking/boss-ranking.component';
import { BossComponent } from './pages/boss/boss.component';
import { AvailableMissionsComponent } from './pages/mission/available-missions/available-missions.component';
import { PerformingMissionsComponent } from './pages/mission/performing-missions/performing-missions.component';
import { GuardService } from './services/auth/guard.service';
import { MessageComponent } from './pages/message/message.component';
import { AccountSettingsComponent } from './pages/account-settings/account-settings.component';
import { ChangePasswordComponent } from './pages/account-settings/change-password/change-password.component';
import { DeleteAccountComponent } from './pages/account-settings/delete-account/delete-account.component';
import { MapComponent } from './pages/map/map.component';
import { ShowMapComponent } from './pages/map/show-map/show-map.component';



const routes: Routes = [
  {path:'',component:HomeComponent},
  {path:'login',component:LoginComponent},
  {path:'register',component:RegisterComponent},
  {path:'boss',component:BossComponent, canActivate: [GuardService], children:
  [
    {path: 'aboutBoss', component: AboutBossComponent},
    {path: 'bossRanking', component: BossRankingComponent},
  ]},
  {path:'agent',component: AgentComponent, canActivate: [GuardService], children:
  [
    {path: 'available', component: AvailableAgentsComponent},
    {path: 'onMission', component: AgentsOnMissionComponent},
    {path: 'forSale', component: AgentsForSaleComponent},
  ]},
  {path:'map',component:MapComponent , canActivate: [GuardService], children:
  [
    {path: 'showMap', component: ShowMapComponent},
  ]},
  {path:'mission',component: MissionComponent, canActivate: [GuardService], children:
  [
    {path: 'performing', component: PerformingMissionsComponent},
    {path: 'available', component: AvailableMissionsComponent},
  ]},
  {path:'message',component: MessageComponent, canActivate: [GuardService], children:
  [
    {path: 'messages', component: ShowMessagesComponent},
    {path: 'reports', component: ShowReportsComponent},
    {path: 'send', component: SendMessageComponent},
  ]},
  {path:'accountSettings',component: AccountSettingsComponent, canActivate: [GuardService], children:
  [
    {path: 'changePassword', component: ChangePasswordComponent},
    {path: 'deleteAccount', component: DeleteAccountComponent},
  ]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
