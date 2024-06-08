import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'rules-dio',
  templateUrl: './rules-dio.component.html',
  styleUrls: ['../rules.component.css']
})
export class RulesDioComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<RulesDioComponent>) { }

  ngOnInit(): void {
  }

}
