import { Component, OnInit } from '@angular/core';
import { NavigationEnd, NavigationStart, Router, Event as RouterEvent } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { UserService } from '../_service/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {

  welcomeTxt: string = 'Pick Fc Home page'
  viewLogin: boolean = false;
  loading: boolean = false;
  
  constructor(private jwtHelper: JwtHelperService, private router: Router, private userService: UserService) {
    this.router.events.subscribe((e: RouterEvent) => {
      this.navIntercept(e);
    });
  }

  ngOnInit(): void {
    if (this.isUserAuthenticated()) {
      this.userService.currentUser().subscribe(x => {
        if (x.verifyTime === null)
          this.viewLogin = true;
        else
          this.viewLogin = false;
          this.router.navigate(['/my-games']);
      }, (err: any) => { console.log(err); });
    } else
      this.viewLogin = true
  }

  public navIntercept(e: RouterEvent): void {
    if (e instanceof NavigationStart)
      this.loading = true;
    if (e instanceof NavigationEnd)
      this.loading = false;
  }

  isUserAuthenticated = (): boolean => {
    const token = localStorage.getItem("jwt");
    var authenticated = token && !this.jwtHelper.isTokenExpired(token) ? true : false;
    return authenticated;
  }
}
