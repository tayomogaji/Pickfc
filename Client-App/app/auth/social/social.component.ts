import { Component, Input, OnInit } from '@angular/core';
import { UserService } from '../../_service/user.service';

declare const FB: any;

@Component({
  selector: 'social',
  templateUrl: './social.component.html',
  styleUrls: ['../auth.component.css']
})
export class SocialComponent implements OnInit {


  @Input() action: string = 'Continue with ';

  constructor(private userSerivce: UserService) { }

  ngOnInit(): void {
  }

  //public facebook(): void {
  //  FB.login(async (res: any) => {
  //    await this.userSerivce.facebook(res.authResponse.accessToken).subscribe(() => { },
  //      (err: any) => { console.log(err); });
  //  }, {scope: 'email'});
  //}


  public facebook(): void {
    FB.login(async (res: any) => {
      //await this.userSerivce.facebook(res.authResponse.accessToken).subscribe(() => { },
      //  (err: any) => { console.log(err); });
    }, {scope: 'email'});
  }



}
