import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { trigger, style, animate, transition } from '@angular/animations';
import { NavigationEnd, NavigationStart, Router, Event as RouterEvent } from '@angular/router';
import { Game } from '../_model/game';
import { GameService } from '../_service/game.service';
import { SetService } from '../_service/set.service';
import { SnackService } from '../_service/snack.service';
import { PlayerService } from '../_service/player.service';
import { TeamService } from '../_service/team.service';
import { tap } from 'rxjs';
import { FileService } from '../_service/file.service';

@Component({
  selector: 'games',
  templateUrl: './games.component.html',
  styleUrls: ['./games.component.css', ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  animations: [
    trigger('fade', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate(1000, style({ opacity: 1 }))
      ]),
    ])
  ]
})

export class GamesComponent implements OnInit {
  
  games = [this.setService.game()];
  game = this.setService.game();
  round = this.setService.round();
  today = new Date();

  page: string = 'list'
  gameType: string = '';
  leaveTitle: string = 'Are you sure you want to leave this game? All progress made will be lost!';

  deadlined: boolean = false;
  loading: boolean = false;

  roundName: string = '';

  id: number = 0;
  maxLife: number = this.playerSerivce.maxLife;
  lifeArr: number[] = [];

  constructor(private cdr: ChangeDetectorRef, private router: Router, private gameService: GameService, private setService: SetService, private teamService: TeamService, private snackService: SnackService, private fileService: FileService, private playerSerivce: PlayerService)
  {
    //this.router.events.pipe(tap(() => (e: RouterEvent) => { this.navIntercept(e); }))
    //this.router.events.subscribe((e: RouterEvent) => {
    //  this.navIntercept(e);
    //});
  }

  ngOnInit(): void {
    this.users();
    this.setLife();
  }

  public pic(pic: string, ph: number): string {
    return this.fileService.pic(pic, ph);
  }

  public navIntercept(e: RouterEvent): void {
    if (e instanceof NavigationStart)
      this.loading = true;
    if (e instanceof NavigationEnd)
      this.loading = false;
  }

  public users(): void {
    this.gameService.users().subscribe(x => {
      this.cdr.markForCheck();
      this.games = x;
    }, (err: any) => {
      console.log(err);
    })
  }

  public play(id: number) {
    this.router.navigate(['/game', id])
  }

  //public teamPic(pic: string): string {
  //  return this.teamService.pic(pic);
  //}

  public hit(): boolean {
    return !this.game.currentPlayer.eliminated && this.game.currentPlayer.hitByID > 0;
  }

  public back($event: any): void {
    this.users();
    //this.view($event, 0);
  }

  public open(game: Game): boolean {
    return this.today >= game.comp.open ? true : false;
  }

  public admin(game: Game,): boolean {
    console.log(game.currentPlayer.admin)
    return game.admins.indexOf(game.currentPlayer) !== null ? true : false;
  }

  public nth(pos: number): string {
    return pos > 0
      ? ["th", "st", "nd", "rd"][
      (pos > 3 && pos < 21) || pos % 10 > 3 ? 0 : pos % 10
      ]
      : "";
  }

  public pickTime(x: number): string {
    return x.toFixed(3);
  }

  public setLife(): void {
    for (let i = 0; i < this.maxLife; i++)
      this.lifeArr.push(i);
  }

  public lifeIcon(life: number, i: number): string {
    return life >= i + 1 ? 'favorite' : 'favorite_border';
  }

  public copy(gamecode: string): void {
    return this.snackService.copied('Game code (' + gamecode + ')')
  }

  public leave(id: number, name: string): void {
    this.playerSerivce.delete(id).subscribe(x => {
      this.users();
      this.cdr.markForCheck();
      this.snackService.leave(name);
    }, (err: any) => {
      console.log(err);
      this.snackService.xleave(name);
    });
  }
}
