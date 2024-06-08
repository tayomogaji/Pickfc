import { Component, OnInit } from '@angular/core';
import { InfoService } from '../_service/info.service';

@Component({
  selector: 'rules',
  templateUrl: './rules.component.html',
  styleUrls: ['./rules.component.css']
})
export class RulesComponent implements OnInit {

  constructor(private infoService: InfoService) { }

  ngOnInit(): void {
  }

  public token(i: number) : string {
    return this.infoService.tokens[i];
  }
}
