import { HeadquartersDetailsComponent } from './page-elements/map-elements/headquarters-details/headquarters-details.component';
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
import { AccountSettingsComponent } from './pages/account-settings/account-settings.component';
import { ChangePasswordComponent } from './pages/account-settings/change-password/change-password.component';
import { DeleteAccountComponent } from './pages/account-settings/delete-account/delete-account.component';
import { MapComponent } from './pages/map/map.component';
import { ShowMapComponent } from './pages/map/show-map/show-map.component';
import { MissionDetailsComponent } from './page-elements/map-elements/mission-details/mission-details.component';
import { MovingAgentsComponent } from './pages/agent/moving-agents/moving-agents.component';
import { ChooseAgentToArrangeAmbushComponent } from './page-elements/ambush/choose-agent-to-arrange-ambush/choose-agent-to-arrange-ambush.component';
import { AmbushDetailsComponent } from './page-elements/map-elements/ambush-details/ambush-details.component';
import { ChooseAgentToPatrolComponent } from './page-elements/agent/choose-agent-to-patrol/choose-agent-to-patrol.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from './utils/token-interceptor';
import { AmbushingAgentsComponent } from './pages/agent/ambushing-agents/ambushing-agents.component';
import { NewsCardComponent } from './page-elements/news/news-card/news-card.component';
import { NewsComponent } from './pages/news/news.component';
import { CreateNewsComponent } from './pages/news/create-news/create-news.component';
import { NotActivatedPlayerComponent } from './page-elements/player/not-activated-player/not-activated-player.component';
import { ForgottenPasswordComponent } from './page-elements/player/forgotten-password/forgotten-password.component';
import { JobComponent } from './pages/job/job.component';
import { ActiveJobsComponent } from './pages/job/active-jobs/active-jobs.component';
import { HowToPlayComponent } from './pages/how-to-play/how-to-play.component';
import { HowToPlayAgentsComponent } from './pages/how-to-play/how-to-play-agents/how-to-play-agents.component';
import { HowToPlayIntroductionComponent } from './pages/how-to-play/how-to-play-introduction/how-to-play-introduction.component';
import { HowToPlayMapComponent } from './pages/how-to-play/how-to-play-map/how-to-play-map.component';
import { MessagePageComponent } from './page-elements/messages/message-page/message-page.component';
import { AgentListComponent } from './pages/agent/agent-list/agent-list.component';
import { AgentActionsComponent } from './page-elements/agent/agent-actions/agent-actions.component';

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
    ShowReportsComponent,
    AccountSettingsComponent,
    ChangePasswordComponent,
    DeleteAccountComponent,
    MapComponent,
    ShowMapComponent,
    HeadquartersDetailsComponent,
    MissionDetailsComponent,
    MovingAgentsComponent,
    ChooseAgentToArrangeAmbushComponent,
    AmbushDetailsComponent,
    ChooseAgentToPatrolComponent,
    AmbushingAgentsComponent,
    NewsCardComponent,
    NewsComponent,
    CreateNewsComponent,
    NotActivatedPlayerComponent,
    ForgottenPasswordComponent,
    JobComponent,
    ActiveJobsComponent,
    HowToPlayComponent,
    HowToPlayAgentsComponent,
    HowToPlayIntroductionComponent,
    HowToPlayMapComponent,
    MessagePageComponent,
    AgentListComponent,
    AgentActionsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MatAutocompleteModule,
    HttpClientModule,
    FormsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter
      }
    }),
    NgbModule
  ],
  providers: [ { provide: ErrorHandler, useClass: CustomErrorHandler },     {
    provide: HTTP_INTERCEPTORS,
    useClass: TokenInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
