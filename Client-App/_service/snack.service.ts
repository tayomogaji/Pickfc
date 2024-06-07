import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class SnackService {

  constructor(private snack: MatSnackBar) { }

  public add(x: string): void {
    this.snack.open('✅ ', x + ' added')
  }

  public xadd(x: string): void {
    this.snack.open('❌', 'Unable to add ' + x)
  }

  public update(x: string): void {
    this.snack.open('✅ ', x + ' updated');
  }

  public xupdate(x: string): void {
    this.snack.open('❌', 'Unable to update ' + x)
  }

  public upload(): void {
    this.snack.open('✅ ' + 'Image uploaded');
  }

  public xupload(): void {
    this.snack.open('❌', ' Unable to upload image');
  }

  public get(x: string): void {
    this.snack.open('Found ' + x);
  }

  public xget(x: string): void {
    this.snack.open('Unable to find ' + x);
  }

  public join(x: string): void {
    this.snack.open('👍', 'Welcome to ' + x);
  }

  public xaccess(x: string) : void {
    this.snack.open('🔒', "You don't have access to this " + x);
  }

  public invalid(): void {
    this.snack.open('❌', 'Invalid code');
  }

  public delete(x: string): void {
    this.snack.open('✅ ', x + ' deleted');
  }

  public xdelete(x: string): void {
    this.snack.open('❌', 'Unable to delete ' + x);
  }

  public reset(x: string): void {
    this.snack.open('✅ ', x + ' resset')
  }

  public xrestart(x: string): void {
    this.snack.open('❌', 'Unable to restart ' + x);
  }

  public hit(x: string): void {
    this.snack.open("You've placed a hit on " + x + '. You can call off this hit from your hit list');
  }

  public hitCallOff(x: string): void {
    this.snack.open('The hit on ' + x + ' has been called off.')
  }

  public hitReveal(a: string, b: string): void {
    this.snack.open(a + ' placed a hit on ' + b);
  }

  public hitWarn(x: string): void {
    this.snack.open('❗ ' + x + ' placed a HIT on you!');
  }

  public eliminated(): void {
    this.snack.open('💀' + " You've been ELIMINATED!")
  }

  public copied(x: string): void {
    this.snack.open(x + ' copied')
  }

  public leave(x: string): void {
    this.snack.open('You have left ' + x);
  }

  public xleave(x: string): void {
    this.snack.open('You were unable to leave ' + x);
  }

  public activated(): void {
    this.snack.open('✅ ' + 'Account activated');
  }

  public resetRequestSent(): void {
    this.snack.open('✅ ' + 'Reset email sent. Please wait...');
  }

  public resetPassword(): void {
    this.snack.open('✅ ' + 'Password updated');
  }

  public xreset(x: string): void {
    this.snack.open('❌', 'Unable to rest ' + x);
  }

  public notified(): void {
    this.snack.open('📧 ' + 'Users already notified');
  }

  public xcurrent(): void {
    this.snack.open('❌', 'Must be current');
  }

  public pickExist(): void {
    this.snack.open('Pick already made');
  }

  public refreshPrompt(): void {
    this.snack.open('Refresh to page and try again');
  }
}
