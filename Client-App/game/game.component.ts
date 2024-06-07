import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { GameService } from '../_service/game.service';
import { SetService } from '../_service/set.service';
import { PlayerService } from '../_service/player.service';
import { SnackService } from '../_service/snack.service';
import { TeamService } from '../_service/team.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { RoundService } from '../_service/round.service';
import { PickService } from '../_service/pick.service';
import { MatSelectionList } from '@angular/material/list'; 
import { Player } from '../_model/player';
import { InfoService } from '../_service/info.service';
import { FileService } from '../_service/file.service';
type ClassResult = 'win' | 'loose' | 'draw';

@Component({
  selector: 'game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class GameComponent implements OnInit {

  id = this.ar.snapshot.params['id'];

  game = this.setService.game();
  comp = this.setService.comp();
  round = this.setService.round();
  player = this.setService.player();
  playerProfile = this.setService.player();
  pick = this.setService.team();
  picks = [this.setService.pick()];
  playerPicks = [this.setService.pick()]

  rounds = [this.setService.round()];
  teams = [this.setService.team()];
  players = [this.setService.player()]
  
  pickedPic: string = '';
  page: string = 'dashboard';
  standings: string[] = ['pos', 'pic', 'name', 'pts', 'pick_time', 'pick', 'result', 'streak', 'status']

  dashboard: boolean = true;
  edit: boolean = false;
  active: boolean = true;
  deadlined: boolean = false;
  viewPicker: boolean = false;
  viewCalendar: boolean = false;
  viewBoost: boolean = false;
  viewBoostDesc: boolean = false;
  viewHits: boolean = false;
  viewHitDesc: boolean = false;
  viewProfile: boolean = false;
  viewSettings: boolean = false;

  maxLife: number = this.playerServcie.maxLife;
  boostMin: number = 0;
  boostMax: number = 0;
  boostTotal: number = 0;
  boostPlayed: number = 0;
  hitsMax: number = 0;

  lifeArr: number[] = [];
  pickedTeams: number[] = [];

  playersData: any;

  myHits: Player[] = [];
  @ViewChild(MatSort) sort = new MatSort();

  constructor(private ar: ActivatedRoute, private router: Router, private cdr: ChangeDetectorRef, private gameService: GameService, private roundService: RoundService, private teamService: TeamService, private playerServcie: PlayerService, private pickService: PickService, private setService: SetService, private snack: SnackService, private fileService: FileService, private infoService: InfoService, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.playerActive();
    this.setLife();
    this.getGame();
  }

  public view(x: string): void {
    switch (x) {
      case x = this.page:
        this.dashboard = true; this.edit = false;
        break;
      case x = 'edit':
        this.dashboard = false; this.edit = true;
    }
  }
  
  public back($event: any): void {
    this.view($event);
  }

  public playerActive(): void {
    this.playerServcie.active(this.id).subscribe(x => {
      this.cdr.markForCheck();
       this.active = x;
    }, (err: any) => {
      console.log(err);
    });
  }

  //public teamPic(p: string): string {
  //  return this.teamService.pic(p);
  //}

  public pic(pic: string, ph: number): string {
    return this.fileService.pic(pic, ph);
  }

  public getGame(): void {
    this.gameService.game(this.id).subscribe(x => {
      this.cdr.markForCheck();
      if (this.active) {
        this.game = x;
        //this.pic = x.pic;
        this.player = x.currentPlayer;
        this.getTeams(x.compID);
        this.getRound(x.comp.roundID);
        this.pickCheck(x.currentPlayer, x.comp.roundID);
        this.players = x.players;
        this.playersData = new MatTableDataSource(x.players);
        this.boostSet();
      } else {
        this.router.navigate(['/games']);
        this.snack.xaccess('game');
      }
    }, (err: any) => {
      console.log(err);
      this.snack.xget('game');
    });
  }

  public getPlayer(id: number): void {
    this.playerServcie.player(id).subscribe(x => {
      this.cdr.markForCheck();
      this.player = x;
      this.boostTotal = x.boostTotal;
      
    }, (err: any) => { console.log(err); })
  }

  public getRound(id: number): void {
    this.roundService.round(id).subscribe(x => {
      this.cdr.markForCheck();
      this.round = x;
      this.deadlined = new Date(x.deadline) <= new Date();
    }, (err: any) => {
      this.snack.xget('round');
      console.log(err);
    });
  }

  public pickCheck(player: Player, roundid: number): void {
    this.pickService.playerPicks(player).subscribe(x => {
      this.picks = x;
      for (let p of x)
        if (p.roundID !== roundid)
          this.pickedTeams.push(p.teamID);
    }, (err: any) => {
      console.log(err);
    });
  }

  public getTeams(compid: number): void {
    this.teamService.teams(compid).subscribe(x => {
      this.cdr.markForCheck();
      this.teams = x;
    }, (err: any) => { console.log(err); })
  }

  public findPlayer(event: Event) {
    const searchValue = (event.target as HTMLInputElement).value;
    this.playersData.filter = searchValue.trim().toLowerCase();
  }

  public fixtures(compid: number): void {
    this.roundService.rounds(compid).subscribe(x => {
      this.rounds = x;
    }, (err: any) => {
      this.snack.xget('rounds for fixtures')
      console.log(err);
    })
    this.viewCalendar = true;
  }

  public setLife(): void {
    for (let i = 0; i < this.maxLife; i++)
      this.lifeArr.push(i);
  }

  public lifeIcon(i: number): string {
    return this.game.currentPlayer.life >= i + 1 ? 'favorite' : 'favorite_border';
  }

  public closeFixtures(): void {
    this.viewCalendar = false;
  }

  public picker(compid: number): void {
    this.roundService.rounds(compid).subscribe(x => {
      this.rounds = x;
    }, (err: any) => {
      this.snack.xget('rounds for fixtures')
      console.log(err);
    })
    this.viewPicker = true;
  }

  public closePicker(): void {
    this.viewPicker = false;
    this.getGame();
  }

  public resultClass(result: string): ClassResult {
    switch (result) {
      case 'W':
        return 'win'
      case 'L':
        return 'loose'
    }
    return 'draw'
  }

  public resultIcon(result: string): string {
    return result === "" ? 'radio_button_unchecked' : 'fiber_manual_record';
  }

  public pickTime(x: number): string {
    return x.toFixed(3)
  }

  public closeBoostView(): void {
    this.viewBoost = false
    this.boostTotal = this.player.boostTotal;
  }

  public boostSet(): void {
    this.boostPlayed = this.player.boostPlayed;
    this.boostTotal = this.player.boostTotal;
    this.boostMax = this.player.boostTotal + this.player.boostPlayed;
    if (this.boostMax === 0)
      this.viewBoostDesc = true;
  }

  public boostChange(plus: boolean): void {
    if (plus) {
      this.boostPlayed++
      this.boostTotal--
    } else {
      this.boostPlayed--
      this.boostTotal++
    }
  }

  public boostUpdate(): void {
    this.player.boostPlayed = this.boostPlayed;
    this.player.boostTotal = this.boostTotal;
    this.playerServcie.edit(this.player).subscribe(() => {
      this.playerServcie.player(this.player.id).subscribe(x => {
        this.player = x;
        this.snack.update('Boost')
      });
    }, (err: any) => {
      this.snack.xupdate('boost')
      console.log(err);
    });
    this.viewBoost = false;
  }

  public closeHitsView(): void {
    this.viewHits = false;
  }

  public callOff(hits: MatSelectionList): void {
    const selected = hits.selectedOptions.selected.map(opt => opt.value);
    
    for (let s of selected) {
      s.hitByID = 0;
      this.myHits.splice(s);
      this.playerServcie.edit(s).subscribe(() => { }, (err: any) => {
        console.log(err);
      });
    }
    this.player.hitsTotal += selected.length;
    this.playerServcie.edit(this.player).subscribe(() => {
      this.getGame();
      this.closeHitsView();
    }, (err: any) => { console.log(err); });

  }

  public hitsSet(): void {
    this.hitsMax = this.player.hitsPlayed + this.player.hitsTotal;
    for (let p of this.players)
      if (p.hitByID === this.player.id && this.myHits.indexOf(p) === -1)
        this.myHits.push(p);

    this.myHits.length === 0 ? this.viewHitDesc = true : this.viewHitDesc = false;
  }

  public validHit(player: Player): boolean {
    var x: boolean = false;
    if (!player.eliminated && player.pos > this.player.pos && !this.deadlined && player.id !== this.player.id && player.hitByID === 0 && this.player.hitsTotal > 0)
      x = true;
    else
      x = false;
    return x;
  }

  public addhit(player: Player) {
    if (this.validHit(player)) {
      this.player.hitsTotal--
      this.playerServcie.edit(this.player).subscribe(() => { }, (err: any) => {
        console.log(err);
      });
      player.hitByID = this.player.id
      this.playerServcie.edit(player).subscribe(() => {
        this.snack.hit(player.name);
        this.getGame();
      }, (err: any) => { console.log(err); })
    }
  }

  public lifeCount(player: Player): string {
    return !player.eliminated ? `${player.life}` : '';
  }

  public profile(player: Player): void {
    this.playerProfile = player;
    this.pickService.playerPicks(player).subscribe(x => {
      this.playerPicks = x;
      const pick = this.playerPicks.findIndex(x => x.id === player.pickID);
      if (pick !== -1 && !this.deadlined && player.id !== this.game.currentPlayer.id)
        this.playerPicks.splice(pick, 1);
    }, (err: any) => { console.log(err); this.snack.xget('picks') });
    this.viewProfile = true;
  }

  public closeProfile(): void {
    this.viewProfile = false;
    this.getGame();
  }

  public closeSettings(): void {
    this.viewSettings = false;
    this.getGame();
  }

  public tokensInfo(i: number): string {
    return this.infoService.tokens[i];
  }

}
