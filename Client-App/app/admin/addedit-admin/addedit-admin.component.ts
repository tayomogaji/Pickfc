import {  ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Team } from '../../_model/team';
import { CompService } from '../../_service/comp.service';
import { FormService } from '../../_service/form.service';
import { SnackService } from '../../_service/snack.service';
import { TeamService } from '../../_service/team.service';
import { RoundService } from '../../_service/round.service';
import { SetService } from '../../_service/set.service';
import { FileService } from '../../_service/file.service';

@Component({
  selector: 'addedit-admin',
  templateUrl: './addedit-admin.component.html',
  styleUrls: ['../admin.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AddeditAdminComponent implements OnInit {

  teamsNum: number = this.data.comp.teamsTotal;
  teamsRemaining: number = this.data.comp.teamsRemaining;
  teamsRemainingTip: string = '';
  today: Date = new Date();
  notify = this.setService.notify();

  compsToAdd: number[] = [];
  compsToDel: number[] = [];
  cr = this.setService.comp();
  rateMax: number = 5;
  rateArr: number[] = [];

  uploaded: boolean = false;
  currentPic: any;
  file: File = new File([''], '');

  compform = this.fb.group({
    _name: this.fs.required(), _open: this.fs.required(), _teamsTotal: this.fs.control(),
  });

  teamform = this.fb.group({
    _name: this.fs.required(), _rating: this.fs.control(), _comps: this.fs.control(),
  });

  roundform = this.fb.group({
    _number: this.fs.control(), _start: this.fs.control(), _deadline: this.fs.control()
  });

  constructor(private fb: FormBuilder, private compService: CompService, private teamService: TeamService, private roundService: RoundService, private setService: SetService, private snack: SnackService, private fileService: FileService, private fs: FormService, private cdr: ChangeDetectorRef, public dialogRef: MatDialogRef<AddeditAdminComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    this.setRate();
    this.compRound();
    this.setRoundNumber();
    this.data.ctr === 'c' ? this.currentPic = this.pic(this.data.comp.pic, 1) : this.currentPic = this.pic(this.data.team.pic, 2);
  }

  public title(): string
  {
    var action = ''
    var type = '';

    switch (this.data.ctr){
      case 'c':
        this.data.comp.id === 0 ? action = 'New ' : action = 'Edit ';
        this.data.comp.id === 0 ? type = 'Competiton ' : type = this.data.comp.name;
        this.notify.comp = true;
        break;
      case 't':
        this.data.team.id === 0 ? action = 'New ' : action = 'Edit '
        this.data.team.id === 0 ? type = 'Team ' : type = this.data.team.name;
        break;
      case 'r':
        this.data.round.id === 0 ? action = this.cr.name : action = 'Edit '
        this.data.round.id === 0 ? type = ': New Round' : type = this.data.round.name;
        this.notify.comp = false;
    }
    return action + type;
  }

  public pic(pic: string, ph: number): string {
    return this.fileService.pic(pic, ph);
  }

  public compRound(): void {
    if (this.data.compid === 0)
      return

    this.compService.comp(this.data.compid).subscribe(x => {
      this.cr = x;
      this.data.round.compID = x.id;
    });
  }
  
  public teamNumber(event: any): void {
    this.teamsNum = event.value;
    this.data.comp.teamsTotal = event.value;
  }

  public addedit(): void {
    switch (this.data.ctr) {
      case 'c': this.compAddedit();
        break;
      case 't': this.teamAddedit();
        break;
      case 'r': this.roundAddedit();
    }
    this.dialogRef.close();
  }

  public compAddedit(): void {
    if (this.compform.valid) {
      this.compService.addedit(this.data.comp).subscribe(x => {
        this.upload('comp', x.id)
        this.data.comp.id === 0 ? this.snack.add('Competition') : this.snack.update('Competition');
      }, (err: any) => {
        this.data.comp.id === 0 ? this.snack.xadd('competition') : this.snack.xupdate('competition');
        console.log(err);
      });
    };
  }

  public teamAddedit(): void {
    if (this.teamform.valid) {
      this.teamService.addedit(this.data.team).subscribe(x => {
        this.upload('Team', x.id)
        this.adddelComps(x);
        if (this.data.team.id === 0) {
          this.snack.add('Team')
          this.data.team.name = '';
        } else { this.snack.update('Team'); this.close() }
      }, (err: any) => {
        this.data.team.id === 0 ? this.snack.xadd('team') : this.snack.xupdate('team');
        console.log(err);
      })
    }
  }

  public roundAddedit(): void {
    if (this.roundform.valid) {
      this.roundService.addedit(this.data.round).subscribe(() => {
        this.data.round.id === 0 ? this.snack.add('Round') : this.snack.update('Round');
      }, (err: any) => {
        this.data.round.id === 0 ? this.snack.xadd('round') : this.snack.xupdate('round'); 1
        console.log(err);
      });
    }
  }

  public setRoundNumber(): void {
    if (this.data.round.id === 0)
      this.data.round.number = this.data.rounds.length + 1;
  }

  public setRate(): void {
    for (let i = 0; i < this.rateMax; i++)
      this.rateArr.push(i);
  }

  public rateIcon(i: number) {
    return this.data.team.rating >= i + 1 ? 'star' : 'star_border';
  }

  public rateClicked(r: number): void {
    this.data.team.rating = r;
  }

  public adddelComps(team: Team): void {
    if (this.compsToAdd.length !== 0)
      for (let compid of this.compsToAdd) {
        team.compID = compid
        this.teamService.compadd(team).subscribe((err: any) => { console.log(err) });
      }

    if (this.compsToDel.length !== 0)
      for (let compid of this.compsToDel)
        this.teamService.compdelete(compid, team.id).subscribe(() => {
          console.log('Success!');
        }, (err: any) => { console.log(err) })
  }

  public compCheck(checked: boolean, id: number): void {
    const i = this.compsToAdd.indexOf(id);
    if (checked) {
      this.compsToAdd.push(id);
      this.compsToDel.splice(i);
    }
    else {
      this.compsToAdd.splice(i);
      this.compsToDel.push(id);
    }
  }

  public upload(content: string, id: number): void {
    if (this.uploaded) {
      this.fileService.upload(this.file, content, id);
      this.uploaded = false;
    };
  }

  public picSelect(event: any): void {
    this.file = event.target.files[0];
    const reader = new FileReader()
    reader.onload = (e: any) => {
      this.currentPic = e.target.result;
      this.cdr.detectChanges();
    }
    reader.readAsDataURL(event.target.files[0]);
    this.uploaded = true;
  }

  public compSelect(e: any) {
    this.data.round.compID = e.value;
  }

  public close(): void {
    this.dialogRef.close();
  }



}
