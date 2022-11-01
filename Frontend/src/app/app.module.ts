import { BossRankingComponent } from './pages/boss/boss-ranking/boss-ranking.component';
import { BrowserModule } from '@angular/platform-browser';
import { ErrorHandler, NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { JwtModule } from '@auth0/angular-jwt';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import {HttpClientModule} from '@angular/common/http';
import {FormsModule} from '@angular/forms';
import { HeaderComponent } from './page-elements/header/header.component';
import { FooterComponent } from './page-elements/footer/footer.component';
import { LoginComponent } from './pages/basic-pages/login/login.component';
import { RegisterComponent } from './pages/basic-pages/register/register.component';
import { HomeComponent } from './pages/basic-pages/home/home.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CustomErrorHandler } from './handlers/customErrorHandler';
import { environment } from 'src/environments/environment';
import { BossComponent } from './pages/boss/boss.component';
import { AboutBossComponent } from './pages/boss/about-boss/about-boss.component';
import { AgentComponent } from './pages/agent/agent.component';
import { AgentsForSaleComponent } from './pages/agent/agents-for-sale/agents-for-sale.component';
import { AgentsOnMissionComponent } from './pages/agent/agents-on-mission/agents-on-mission.component';
import { AvailableAgentsComponent } from './pages/agent/available-agents/available-agents.component';
import { MissionComponent } from './pages/mission/mission.component';
import { AvailableMissionsComponent } from './pages/mission/available-missions/available-missions.component';
import { PerformingMissionsComponent } from './pages/mission/performing-missions/performing-missions.component';
import { MissionCardComponent } from './page-elements/mission/mission-card/mission-card.component';
import { ChooseAgentOnMissionComponent } from './page-elements/agent/choose-agent-on-mission/choose-agent-on-mission.component';
import { MessageComponent } from './pages/message/message.component';
import { SendMessageComponent } from './pages/message/send-message/send-message.component';
import { ShowMessagesComponent } from './pages/message/show-messages/show-messages.component';
import { ShowReportsComponent } from './pages/message/show-reports/show-reports.component';

export function tokenGetter(){
  return sessionStorage.getItem("jwtToken");
}

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    LoginComponent,
    RegisterComponent,
    HomeComponent,
    AboutBossComponent,
    BossComponent,
    BossRankingComponent,
    AgentComponent,
    AgentsForSaleComponent,
    AgentsOnMissionComponent,
    AvailableAgentsComponent,
    MissionComponent,
    AvailableMissionsComponent,
    PerformingMissionsComponent,
    MissionCardComponent,
    ChooseAgentOnMissionComponent,
    MessageComponent,
    SendMessageComponent,
    ShowMessagesComponent,
    ShowReportsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MatAutocompleteModule,
    HttpClientModule,
    FormsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: [environment.APIEndpoint],
        disallowedRoutes: []
      }
    }),
    NgbModule
  ],
  providers: [ { provide: ErrorHandler, useClass: CustomErrorHandler }],
  bootstrap: [AppComponent]
})
export class AppModule { }
