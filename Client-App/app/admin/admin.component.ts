import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { Comp } from '../_model/comp';
import { Team } from '../_model/team';
import { Round } from '../_model/round';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DeleteComponent } from '../delete/delete.component';
import { UploadComponent } from '../upload/upload.component';
import { CompService } from '../_service/comp.service';
import { FileService } from '../_service/file.service';
import { SetService } from '../_service/set.service';
import { TeamService } from '../_service/team.service';
import { AddeditAdminComponent } from './addedit-admin/addedit-admin.component';
import { RoundService } from '../_service/round.service';
import { FixtureComponent } from './fixture/fixture.component';
import { NzMessageService } from 'ng-zorro-antd/message';
import { MailService } from '../_service/mail.service';
import { SnackService } from '../_service/snack.service';
import { isContext } from 'vm';


@Component({
  selector: 'admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  comp = this.setService.comp();
  dcomp = this.setService.comp();
  comps = [this.setService.comp()];
  compsCol: string[] = ['pic', 'name', 'opens', 'teams', 'default', 'active', 'reset', 'edit', 'remove'];
  

  compid: number = 0;
  compTeamid: number = 0;
  compsData: any;

  compTeamTitle: string = '';
  compRoundTitle: string = '';

  team = this.setService.team();
  teams = [this.setService.team()];
  teamsCol: string[] = ['pic', 'name', 'rating', 'competitions', 'edit', 'remove'];
  teamsData: any;

  round = this.setService.round();
  rounds = [this.setService.round()];
  roundsCol: string[] = ['round','start', 'deadline', 'current', 'show', 'fixtures', 'edit', 'remove'];
  roundsData: any;

  notify = this.setService.notify();

  rateMax: number = 5;
  rateArr: number[] = [];

  visible: boolean = false;

  @ViewChild(MatSort) sort = new MatSort();
  file: File = new File([''], '')

  constructor(private cdr: ChangeDetectorRef, public dialog: MatDialog, private compService: CompService, private teamService: TeamService, public roundService: RoundService, private mailService: MailService, private setService: SetService, private snack: SnackService, private fileService: FileService, public nzMsgService: NzMessageService) { }

  ngOnInit(): void {
    if (this.comps.length !== 0) {
      this.getComps();
      this.getDefaualtComp();
    }
    

    this.setRate();
  }

  public open(): void {
    this.visible = true;
  }

  public close(): void {
    this.visible = false;
  }

  public getComp(id: number): void {
    this.compService.comp(id).subscribe(x => {
      this.comp = x;
      this.cdr.markForCheck();
    }, (err: any) => { console.log(err) })
  }

  public getDefaualtComp(): void {
    this.compService.default().subscribe(x => {
      this.dcomp = x;
      this.compid
      this.compRounds(x.id, x.name);
      this.compTeams(x.id, x.name);
      this.cdr.markForCheck();
    }, (err: any) => {
      console.log(err);
    })
  }

  public getComps(): void {
    this.compService.comps().subscribe(x => {
      this.comps = x;
      this.compsData = new MatTableDataSource(x)
      this.cdr.markForCheck();
    }, (err: any) => { console.log(err); })
  }

  public getTeam(id: number): Team {
    var t = this.team;
    this.teamService.team(id).subscribe(x => {
      t = x;
      this.cdr.markForCheck();
    }, (err: any) => { console.log(err); })
    return t;
  }

  public getTeams(compid: number): void {
    this.teamService.teams(compid).subscribe(x => {
      this.teams = x
      this.cdr.markForCheck();
      //this.compTeamTitle = 'All';
      this.teamsData = new MatTableDataSource(x)
      this.teamsData.sort = this.sort;
    }, (err: any) => { console.log(err); })
  }

  public getRounds(compid: number): void {
    this.roundService.rounds(compid).subscribe(x => {
      this.rounds = x;
      this.cdr.markForCheck();
      this.roundsData = new MatTableDataSource(x);
      this.roundsData.sort = this.sort;
    }, (err: any) => { console.log(err); })
  }

  public compTeams(id: number, name: string): void {
    this.compid = id;
    this.compTeamTitle = name;
    this.getTeams(id);
  }

  //public compTeams(id: number, name: string): void {
  //  this.compTeamid = id;
  //  this.compTeamTitle = name;
  //  this.teamService.teams(id).subscribe(x => {
  //    this.teams = x
  //    this.cdr.markForCheck();
  //    this.teamsData = new MatTableDataSource(x);
  //    this.teamsData.sort = this.sort;
  //  }, (err: any) => { console.log(err) })
  //}

  public compRounds(id: number, name: string): void{
    this.compid = id
    this.compRoundTitle = name;
    this.getRounds(id);
  }

  public compReset(comp: Comp): void {
    this.compService.reset(comp).subscribe(() => {
      this.snack.reset('Comp');
    }, (err: any) => {
      this.snack.xreset('comp');
      console.log(err);
    });
  }

  public addedit(comp: Comp, team: Team, round: Round, ctr: string): void
  {
    for (let c of this.comps) {
      this.compService.hasTeam(c.id, team.id).subscribe(x => {
        c.hasTeam = x;
      }, (err: any) => { console.log(err) });
    }

    const dialogRef = this.dialog.open(AddeditAdminComponent, {
      autoFocus: false,
      maxHeight: '90vh',
      data: { comp: comp, team: team, round: round, teams: this.teams, comps: this.comps, rounds: this.rounds, compid: this.compid, ctr: ctr }
    });

    dialogRef.afterClosed().subscribe(() => {
      this.listRefresh();
    }, (err: any) => {
      console.log(err);
    });
  }

  public fixtures(round: Round): void
  {
    this.teamService.teams(round.compID).subscribe(x => {
      const dialogRef = this.dialog.open(FixtureComponent, {
        autoFocus: false,
        maxHeight: '90vh',
        data: { round: round, teams: x }
      });

      dialogRef.afterClosed().subscribe(() => {
        console.log('Testing fixtures');
      }, (err: any) => { console.log(err); })
    }, (err: any) => { console.log(err); });
  }

  public delete(comp: Comp, team: Team, round: Round, type: string): void
  {
    var id: number = 0;
    var name: string = '';
    switch (type) {
      case 'c': id = comp.id;
        name = comp.name;
        break;
      case 't':
        id = team.id;
        name = team.name;
        break;
      case 'r':
        id = round.id;
        name = round.name;
    }

    const dialogRef = this.dialog.open(DeleteComponent, {
      autoFocus: false,
      maxHeight: '60vh',
      data: { comp: comp, team: team, type: type, id: id, name: name}
    });
    dialogRef.afterClosed().subscribe(() => {
      this.listRefresh();
    }, (err: any) => {
      console.log(err);
    });
  }

  public compActive(checked: boolean, comp: Comp): void {
    comp.active = checked;
    this.compService.addedit(comp).subscribe((err: any) => { console.log(err); })
  }

  public teamsDone(comp: Comp): boolean {
    return comp.teamsCount >= comp.teamsTotal;
  }

  public teamsTip(comp: Comp): string {
    var tip: string = '';
    if (comp.teamsRemaining === 0) {
      tip;
    } else if (comp.teamsCount < comp.teamsTotal) {
      tip = comp.teamsRemaining === 1 ? comp.teamsRemaining + ' team needed to activate' : comp.teamsRemaining + ' teams needed to activate';
    }
    return tip;
  }

  public searchTeam(event: Event) {
    const searchValue = (event.target as HTMLInputElement).value;
    this.teamsData.filter = searchValue.trim().toLowerCase();
  }

  public setRate(): void {
    for (let i = 0; i < this.rateMax; i++)
      this.rateArr.push(i);
  }

  public rating(team: Team): number[] {
    const rateArr: number[] = [];
    for (let i = 0; i < team.rating; i++)
      rateArr.push(i)

    return rateArr;
  }

  public rateIcon(i: number) {
    return this.team.rating >= i + 1 ? 'star' : 'star_border';
  }

  public rateClicked(r: number, team: Team): void {
    console.log(r);
    team.rating = r;
    this.teamService.addedit(team);
  }

  public defaultSelect(e: any): void {
    this.compService.defaultSwitch(e.value).subscribe(() => {
      this.compid = e.value;
    }, (err: any) => {
      console.log(err);
    });
  }

  public listRefresh(): void {
    this.getComps();
    this.getTeams(this.compid);
    this.getRounds(this.compid);
  }

  public setShow(round: Round): void {
    this.roundService.addedit(round).subscribe(() => {}, (err: any) => { err });
  }

  public setCurrent(round: Round): void {
    this.nzMsgService.info('click confirm');
    this.roundService.setCurrentRound(round).subscribe(() => {
      this.getRounds(round.compID);
    }, (err: any) => { console.log(err) });
  }

  public closePop(): void {
    this.nzMsgService.info('click cancel');
  }

  public setOK(x: boolean): string {
    return x ? 'done' : 'horizontal_rule';
  }

  public newContentNotify(id: number, isComp: boolean, notified: boolean, current: boolean): void {
    if (current) {
      if (!notified) {
        this.notify.id = id;
        this.notify.comp = isComp;
        this.mailService.newContent(this.notify).subscribe(() => {
          this.listRefresh();
        }, (err: any) => { console.log(err); });
      } else { this.snack.notified(); }
    } else {
      this.snack.xcurrent();
    } 
  }

  public deadlineNotify(round: Round): void {
    if (round.current) {
      if (!round.deadlineNotified) {
        this.notify.id = round.id
        this.notify.comp = false;
        this.mailService.roundDeadline(this.notify).subscribe(() => {
          this.listRefresh();
        }, (err: any) => { console.log(err); });
      } else { this.snack.notified(); }
    } else {
      this.snack.xcurrent();
    }
    
  }

  public notifyTitle(notified: boolean, result: string): string {
    return notified ? "Users notified" : 'Email users that this ' + result;
  }

  public pic(pic: string, ph: number): string {
    return this.fileService.pic(pic, ph);
  }

  public upload(comp: Comp, team: Team, isComp: boolean): void
  {
    const id = isComp ? comp.id : team.id;
    const name = isComp ? comp.name : team.name;
    const pic = isComp ? this.pic(comp.pic, 1) : this.pic(team.pic, 2);
    const content = isComp ? 'Comp' : 'Team';
    team.compID = id;

    const dialogRef = this.dialog.open(UploadComponent, {
      data: {
        id: id, name: name, pic: pic, content: content
      },
    });

    dialogRef.afterClosed().subscribe(() => {
      this.listRefresh();
    }, (err: any) => {
      console.log(err);
    });
  }
}
