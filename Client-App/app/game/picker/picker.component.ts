import { ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Team } from '../../_model/team';
import { FileService } from '../../_service/file.service';
import { SetService } from '../../_service/set.service';
import { FixtureService } from '../../_service/fixture.service';
import { PickService } from '../../_service/pick.service';
import { SnackService } from '../../_service/snack.service';
import { Round } from '../../_model/round';
import { TeamService } from '../../_service/team.service';
import { animate, style, transition, trigger } from '@angular/animations';
type ClassName = 'win' | 'loose' | 'draw';

@Component({
  selector: 'picker',
  templateUrl: './picker.component.html',
  styleUrls: ['./picker.component.css', '../game.component.css'],
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
export class PickerComponent implements OnInit {

  pick = this.setService.pick();
  @Input() picks = [this.setService.pick()];
  fixture = this.setService.fixture();

  selected: number = 0;
  picked: number = 0;

  validPick: boolean = false;
  pickChange: boolean = true;
  today: Date = new Date();

  @Input() round = this.setService.round();
  @Input() player = this.setService.player();
  @Input() teams = [this.setService.team()];
  @Input() rounds = [this.setService.round()];
  @Input() gameid = 0;
  @Input() pickedTeams: number[] = [];
  @Input() deadlined: boolean = false;
  started: boolean = false;

  @Output() closePicker = new EventEmitter<any>();

  constructor(private pickService: PickService, private teamService: TeamService, private fixtureService: FixtureService, private snackService: SnackService, private cdr: ChangeDetectorRef, private setService: SetService, private fileService: FileService) { }

  ngOnInit(): void {
    this.setPicker();
  }

  public setPicker(): void {
    this.pick.id = this.player.pickID;
    this.pick.team = this.player.pick.team;
    this.getFixture(this.pick.team);
    this.pick.roundID = this.round.id;
    this.pick.roundNumber = this.round.number;
    this.pick.gameID = this.gameid;
    this.pick.playerID = this.player.id;
    this.pick.playerName = this.player.name;
    this.started = new Date() >= new Date(this.round.start);

    this.picked = this.player.pick.teamID;
    this.selected = this.player.pick.teamID;
  }

  public addedit(): void {
    if (!this.deadlined) {
      this.pickService.pickExist(this.pick).subscribe(x => {
        if (x && this.pick.id === 0) {
          this.closePicker.emit();
          this.snackService.pickExist();
        } else {
          this.pick.time = new Date();
          this.pickService.addedit(this.pick).subscribe(() => {
            this.snackService.update('Pick');
            this.player.pick = this.pick
            this.closePicker.emit();
          }, (err: any) => {
            this.snackService.xupdate('pick');
            console.log(err);
          });
        }
      }, (err: any) => { console.log(err); })
    }
  }

  public pic(pic: string, ph: number): string {
    return this.fileService.pic(pic, ph);
  }

  public getFixture(team: Team): void {
    if (this.round.id !== 0 || team.id !== 0) {
      this.fixtureService.teamFixture(team.id, this.round.id).subscribe(x => {
        this.cdr.markForCheck();
        this.fixture = x;
        this.pick.roundID = x.roundID;
      }, (err: any) => { console.log(err); })
    }
  }

  public rating(team: Team): number[] {
    const rateArr: number[] = [];
    for (let i = 0; i < team.rating; i++)
      rateArr.push(i)

    return rateArr;
  }

  public select(team: Team): void {
    this.pickChange = !this.pickChange;
    if (this.pickedTeams.indexOf(team.id) === -1 && team.hasFixture
      && !this.deadlined && this.started && !this.player.eliminated) {
      this.pick.team = team;
      this.pick.teamID = team.id;
      this.selected = team.id;
      this.getFixture(this.pick.team)
      this.validPick = this.picked !== this.pick.teamID;
    }
  }

  public resultClass(result: string): ClassName {
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

  public toStart(d: Date): boolean {
    return this.today < new Date(d);
  }

  public pastStart(round: Round): string {
    var start = new Date(round.start);
    var dl = new Date(round.deadline);

    if (this.today > start && this.today < dl)
      return 'Current'
    else
      return 'Completed'
  }
}
