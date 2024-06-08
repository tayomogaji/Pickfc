import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder} from '@angular/forms';
import { UserService } from 'src/app/_service/user.service';
import { FormService } from '../../_service/form.service';
import { SetService } from '../../_service/set.service';

@Component({
  selector: 'signup',
  templateUrl: './signup.component.html',
  styleUrls: ['../auth.component.css']
})
export class SignupComponent implements OnInit {

  user = this.setService.user();
  exist: boolean = false;
  hide = true;

  verifyTitle: string = "Activate account";
  verifyEmail: string = "the email address used to create your account"
  verify: boolean = false;
  loading: boolean = false;
  
  form = this.fb.group({
    firstname_input: this.fs.required(),
    lastname_input: this.fs.required(),
    email_input: this.fs.email(),
    password_input: this.fs.password(),
    confirm_input: [''],
    photo_input: ['']
  }, {
    validators: this.fs.match('password_input','confirm_input')
  });

  constructor(private userService: UserService, private setService: SetService, private fs: FormService, private router: Router, private fb: FormBuilder) { }

  ngOnInit(): void { }

  //picSelected(event: any): void {
  //  this.user.pic = event.target.file[0];
  //}

  public close(): void {
    this.verify = false;
    this.router.navigate(['']);
  }

  signup(): void {
    this.loading = true;
    this.userService.exist(this.user.email).subscribe(x => {
      this.exist = x;
      if (!x) {
        if (this.form.valid) {
          this.user.fullName = this.user.firstName + ' ' + this.user.lastName;
          this.verifyEmail = this.user.email;
          this.userService.addedit(this.user).subscribe({
            next: () => {
              console.log("User added")
              this.verify = true;
            }, error: (err: any) => { console.log(err) }
          });
        }
      }
    }, (err: any) => { console.log(err); });
    this.loading = false;
  }

}
