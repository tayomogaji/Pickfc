import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { JwtModule } from '@auth0/angular-jwt';
import { AppRoutingModule } from './app-routing.module';
import { MaterialModule } from './material.module';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './auth/login/login.component';
import { AuthGuard } from './auth/auth.guard';
import { SignupComponent } from './auth/signup/signup.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { GameComponent } from './game/game.component';
import { GamesComponent } from './games/games.component';
import { AdminComponent } from './admin/admin.component';
import { JoinComponent } from './game/join/join.component'
import { AddEditGameComponent } from './game/addedit-game/addedit-game.component';
import { UploadComponent } from './upload/upload.component';
import { AddeditAdminComponent } from './admin/addedit-admin/addedit-admin.component';
import { DeleteComponent } from './delete/delete.component';
import { PickerComponent } from './game/picker/picker.component';
import { ZorroModule } from './zorro.module';
import { FixtureComponent } from './admin/fixture/fixture.component';
import { GameSettingsComponent } from './game/game-settings/game-settings.component';
import { CountdownConfig, CountdownGlobalConfig, CountdownModule } from 'ngx-countdown';
import { ProfileComponent } from './game/profile/profile.component';
import { ClipboardModule } from '@angular/cdk/clipboard';
import { UserComponent } from './user/user.component';
import { VerifyComponent } from './auth/verify/verify.component';
import { RulesComponent } from './rules/rules.component';
import { SocialComponent } from './auth/social/social.component';
import { CoolSocialLoginButtonsModule } from '@angular-cool/social-login-buttons';
import { RulesPageComponent } from './rules/rules-page/rules-page.component';
import { RulesDioComponent } from './rules/rules-dio/rules-dio.component';
import { ArtComponent } from './art/art.component';
import { LoadingComponent } from './loading/loading.component';

export function tokenGetter(){
  return localStorage.getItem('jwt');
}

function countdownConfigFactory(): CountdownConfig {
  return { format: `h:mm:ss` };
}

@NgModule({
  declarations: [
    AppComponent,
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent,
    SignupComponent,
    GamesComponent,
    GameComponent,
    AddEditGameComponent,
    AdminComponent,
    JoinComponent,
    UploadComponent,
    AddeditAdminComponent,
    DeleteComponent,
    PickerComponent,
    FixtureComponent,
    GameSettingsComponent,
    ProfileComponent,
    UserComponent,
    VerifyComponent,
    RulesComponent,
    SocialComponent,
    RulesPageComponent,
    RulesDioComponent,
    ArtComponent,
    LoadingComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    MaterialModule,
    ZorroModule,
    CountdownModule,
    ClipboardModule,
    CoolSocialLoginButtonsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:7157", "localhost:4200", "pickfc.com", "pickfc.azurewebsites.net"],
        disallowedRoutes: []
      }
    }),
  ],
  providers: [
    AuthGuard,
    { provide: CountdownGlobalConfig, useFactory: countdownConfigFactory }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
