import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ArtComponent } from '../art/art.component';
import { RulesDioComponent } from '../rules/rules-dio/rules-dio.component';
import { UserComponent } from '../user/user.component';
import { SetService } from '../_service/set.service';
import { UserService } from '../_service/user.service';

@Component({
  selector: 'nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class NavMenuComponent implements OnInit{

  isExpanded = false;
  user = this.setService.user();

  constructor(private route: Router, private cdr: ChangeDetectorRef, private jwtHelper: JwtHelperService, private userService: UserService, private setService: SetService, private dialog:MatDialog) { }

  ngOnInit(): void {
    if (this.isUserAuthenticated())
      this.getUser();
  }

  isUserAuthenticated = (): boolean => {
    const token = localStorage.getItem("jwt");
    var authenticated = token && !this.jwtHelper.isTokenExpired(token) ? true : false;
    this.cdr.markForCheck();
    return authenticated;
  }

  public getUser(): void {
    this.userService.currentUser().subscribe(x => {
      this.cdr.markForCheck();
      this.user = x;
    }), (err: any) => {
      console.log(err);
    }
  }

  public collapse() {
    this.isExpanded = false;
  }

  public toggle() {
    this.isExpanded = !this.isExpanded;
  }

  public logout = () => {
    localStorage.removeItem("jwt");
    this.route.navigate(['']);
    this.user = this.setService.user();
  }

  public account(): void {
    const dialogRef = this.dialog.open(UserComponent, {
      data: { user: this.user }
    });
    dialogRef.afterClosed().subscribe(() => {
      this.cdr.markForCheck();
      this.getUser();
    }, (err: any) => { console.log(err); })
  }

  public rules(): void {
    this.dialog.open(RulesDioComponent, {
      maxWidth: '70vh',
      maxHeight: '90vh',
    });
  }

  public icons(): void {
    const dialogRef = this.dialog.open(ArtComponent, {
      maxHeight: '90vh',
      minWidth:'40vh',
      maxWidth:'40vh',
      data: { user: this.user }
    });
    dialogRef.afterClosed().subscribe(() => {
      this.cdr.markForCheck();
      this.getUser();
    }, (err: any) => {
      console.log(err);
    })
  }

}
