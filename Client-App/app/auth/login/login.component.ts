import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router, Event as RouterEvent, NavigationStart, NavigationEnd } from '@angular/router';
import { UserService } from '../../_service/user.service';
import { FormService } from '../../_service/form.service';
import { SetService } from '../../_service/set.service';
import { SnackService } from '../../_service/snack.service';
import { Auth } from '../../_model/auth';
import { MailService } from '../../_service/mail.service';
import { tap } from 'rxjs';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['../auth.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoginComponent implements OnInit {

  auth = this.setService.auth();
  user = this.setService.user();

  verify: boolean = false;
  validUser: boolean = false;
  invalidLogin: boolean = false;
  pwChange: boolean = false;
  resetPw: boolean = true;
  loading: boolean = false;

  title: string = '';
  verifyTitle: string = "Activate Account";

  form = this.fb.group({
    email: this.fs.email(),
    password: this.fs.required(),
    rememberMe: this.fs.control()
  });

  resetRequestForm = this.fb.group({
    email: this.fs.email(),
  });

  constructor(private router: Router, private userService: UserService, private mailService: MailService, private setService: SetService, private snack: SnackService, private fs: FormService, private fb: FormBuilder) {
    this.router.events.subscribe((e: RouterEvent) => {
      this.navIntercept(e);
    });
  }

  ngOnInit(): void {
    this.loginView(false);
  }

  public navIntercept(e: RouterEvent): void {
    if (e instanceof NavigationStart)
      this.loading = true;
    if (e instanceof NavigationEnd)
      this.loading = false;
  }

  public loginView(hide: boolean): void {
    this.title = 'Login'
    this.pwChange = hide;
    this.verify = hide;
  }

  public close(): void {
    this.loginView(false);
  }

  public back(e: any): void {
    this.loginView(e);
    this.auth = this.setService.auth();
  }

  public forgotPw(): void {
    this.pwChange = true;
    this.title = 'Reset Password';
    this.verifyTitle = this.title;
  }

  public verifyAccount(): void {
    this.loading = true;
    if (this.form.valid) {
      this.userService.exist(this.auth.email)
        .pipe(tap(() => this.loading = true))
        .subscribe(x => {
        if (x) {
          this.userService.viaEmail(this.auth.email).subscribe(x => {
            if (x.verifyTime === null) {
              this.resetPw = false;
              this.verify = true;
            } else {
              this.login(this.auth);
            }
          }, (err: any) => { console.log(err); });
        } else { this.invalidLogin = true; }
      }, (err: any) => {console.log(err);});
    }
    this.loading = false
  }

  public codeRequest(): void {
    this.auth.activationCode = false;
    this.mailService.codeRequest(this.auth).subscribe(() => {
      this.verify = true;
      this.snack.resetRequestSent();
    }, (err: any) => { console.log(err); })
  }

  public login(auth: Auth): void {
    this.userService.login(auth).subscribe({
      next: (x) => {
        localStorage.setItem("jwt", x.token);
        this.invalidLogin = false;
        this.resetPw = true;
        this.verify = false;
        this.router.navigate(['/my-games']);
      },
      error: (err: any) => {
        this.invalidLogin = true;
        console.log(err);
      }
    });
  }
}
