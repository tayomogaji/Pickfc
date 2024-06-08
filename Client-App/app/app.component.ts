import { Component } from '@angular/core';
import { Router, Event as RouterEvent, NavigationStart, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'Pick Fc';
  loading: boolean = false;

  constructor(private router: Router) {
    this.router.events.subscribe((e: RouterEvent) => {
      this.navIntercept(e);
    });
  }

  public navIntercept(e: RouterEvent): void {
    if (e instanceof NavigationStart)
      this.loading = true;
    if (e instanceof NavigationEnd)
      this.loading = false;
  }
}


