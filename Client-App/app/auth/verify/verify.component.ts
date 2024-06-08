import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { Auth } from '../../_model/auth';
import { User } from '../../_model/user';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { FormService } from '../../_service/form.service';
import { MailService } from '../../_service/mail.service';
import { SetService } from '../../_service/set.service';
import { SnackService } from '../../_service/snack.service';
import { UserService } from '../../_service/user.service';

@Component({
  selector: 'verify',
  templateUrl: './verify.component.html',
  styleUrls: ['../auth.component.css']
})
export class VerifyComponent implements OnInit {

  user = this.setService.user();
  validCode = true;

  title: string = '';
  codeType: string = '';
  outcome: string = '';
  submit: string = '';

  viewPasswordReset: boolean = false;
  loading: boolean = false;

  @Input() auth = this.setService.auth();
  @Input() email: string = '';
  @Input() resetPw: boolean = false;
  @Output() back = new EventEmitter<boolean>();

  form = this.fb.group({
    code_inpt: this.fs.required()
  });
  passwordForm = this.fb.group({
    new_password: this.fs.password(), confirm_password: ['']
  }, {
    validators: this.fs.match('new_password', 'confirm_password')
  });

  constructor(private userService: UserService, private mailService: MailService, private setService: SetService, private fb: FormBuilder, private fs: FormService, private router: Router, private snack: SnackService) { }

  ngOnInit(): void {
    this.verifySet();
  }

  public close(): void {
    this.back.emit(false);
  }

  public verifySet(): void {
    if (this.resetPw) {
      this.title = 'Reset Password';
      this.codeType = 'A password reset';
      this.outcome = 'reset your password';
      this.submit = 'Reset';
    } else {
      this.title = 'Activate Account';
      this.codeType = 'An activation';
      this.outcome = 'activate your account';
      this.submit = 'Activate'
    }
  }

  public activateReset(): void {
    this.loading = true;
    if (this.form.valid) {
      this.userService.verifiedExist(this.auth.code).subscribe(x => {
        if (x) {
          this.userService.viaCode(this.auth.code).subscribe(x => {
            if (this.resetPw) {
              this.user = x;
              this.user.password = '';
              this.viewPasswordReset = true;
            } else {
              this.userService.verify(this.auth).subscribe(() => {
                this.snack.activated();
                this.login(this.setAuth(x));
              }, (err: any) => { console.log(err); });
            }
          }, (err: any) => { console.log(err); });
        } else { this.snack.invalid(); }
      }, (err: any) => { console.log(err); })
    }
    this.loading = false;
  }

  public recipient(): string {
    return this.auth.email === '' ? 'your email address' : this.auth.email;
  }

  public codeRequest(): void {
    this.auth.activationCode = true;
    this.mailService.codeRequest(this.auth).subscribe(() => {
      this.snack.resetRequestSent();
    }, (err: any) => { console.log(err); })
  }

  public resetPassword(): void {
    if (this.passwordForm.valid) {
      this.userService.addedit(this.user).subscribe(x => {
        this.snack.resetPassword();
        this.login(this.setAuth(x));
        //this.close();
      }, (err: any) => {
        this.snack.xreset('password');
        console.log(err);
      });
    }
  }

  public setAuth(user: User): Auth {
    var auth = this.setService.auth();
    auth.email = user.email;
    auth.password = user.password;
    auth.rememberMe = true;
    return auth;
  }

  public login(auth: Auth): void {
    this.userService.login(auth).subscribe({
      next: (x) => {
        localStorage.setItem("jwt", x.token);
        this.resetPw = true;
        this.router.navigate(['/my-games']);
      },
      error: (err: any) => {
        console.log(err);
      }
    });
  }
}
