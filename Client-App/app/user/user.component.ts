import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { FileService } from '../_service/file.service';
import { FormService } from '../_service/form.service';
import { SnackService } from '../_service/snack.service';
import { UserService } from '../_service/user.service';

@Component({
  selector: 'user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserComponent implements OnInit {

  pic: any;

  resetPwTxt: string = '';
  confirmedPw: string = '';
  resetPw: boolean = false;
  uploaded: boolean = false;
  viewDelete: boolean = false;

  form = this.fb.group({
    _firstname: this.fs.required(), _lastname: this.fs.required(), _currentPw: [''], _newPw: [''], _confirmPw: ['']
  }, {
    validators: this.fs.match('_newPw', '_confirmPw')
  });

  pwCurrent = this.form.get('_current');
  pwNew = this.form.get("_newPw");
  pwConfirm = this.form.get('_confirmPw');

  constructor(private userService: UserService, private fb: FormBuilder, private fs: FormService, private snack: SnackService, private fileService: FileService, private cdr: ChangeDetectorRef, private route: Router, public dialogRef: MatDialogRef<UserComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    if (this.data.user.id === 0)
      window.location.reload();
    this.setPasswords();
  }

  public notify(x: boolean): string {
    return x ? "Notify me" : "Don't notify me";
  }

  public setPasswords(): void {
    this.pwTxtInfo();
    this.pwNew?.disable();
    this.pwConfirm?.disable();
  }

  public accountTitle(): string {
    return this.viewDelete ? 'Delete' : 'My';
  }

  public currentPw(event: any): void {
    var current = this.form.get('_currentPw');
    if (!event.shiftKey && (event.key === 'Tab' || event.key === 'Enter'))
      if (current?.value === this.data.user.password) {
        this.resetPw = true;
        this.pwNew?.addValidators(this.fs.passPattern());
        this.pwNew?.addValidators(this.fs.require());
        this.pwNew?.enable();
        this.pwConfirm?.enable();
      } else {
        this.resetPw = false;
        this.pwNew?.clearValidators();
        this.pwNew?.disable();
        this.pwConfirm?.disable();
        this.pwNew?.setValue('');
        this.pwConfirm?.setValue('')
      }
    this.pwTxtInfo();
  }

  public pwTxtInfo(): string {
    const keys: string = 'then press "Tab" or "Enter"'
    return this.resetPw ? 'To cancel this password change, leave the Current Password field empty, ' + keys : 'To change your password, enter your Current one ' + keys;
  }

  public done(): void {
    if (this.data.user.id > 0) {
      if (this.form.valid) {
        if (this.confirmedPw !== '')
          this.data.user.password = this.confirmedPw;
        this.userService.addedit(this.data.user).subscribe(() => {
          this.snack.update('Account details');
          this.close();
        }, (err: any) => {
          console.log(err);
          this.snack.xupdate('account details.');
        })
      } else {
        console.log('form invalid');
      }
    } else { window.location.reload(); }

  }

  public close(): void {
    this.viewDelete = false;
    this.dialogRef.close();
  }

  public deletePrompt(): void {
    this.viewDelete = true;
  }

  public delete(): void {
    if (this.data.user.id > 0) {
      this.userService.delete(this.data.user.id).subscribe(() => {
        this.close();
        localStorage.removeItem("jwt");
        this.route.navigate(['']);
        this.snack.delete('Your account');
      }, (err: any) => {
        (console.log(err));
        this.snack.xdelete('your account.');
      });
    } else { window.location.reload(); }
  }
}


