import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CompService } from '../_service/comp.service';
import { GameService } from '../_service/game.service';
import { RoundService } from '../_service/round.service';
import { SetService } from '../_service/set.service';
import { SnackService } from '../_service/snack.service';
import { TeamService } from '../_service/team.service';

@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DeleteComponent implements OnInit {

  removeTeams: boolean = false;
  teams = [this.setService.team()]

  constructor(private compService: CompService, private teamService: TeamService, private roundService: RoundService, private gameService: GameService, private setService: SetService, private cdr: ChangeDetectorRef, private snack: SnackService, public dialogRef: MatDialogRef<DeleteComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
  }

  public close(): void {
    this.dialogRef.close();
  }

  public teamsRemoval(event: boolean): void {
    this.removeTeams = event;
    console.log('removing teams? ' + this.removeTeams);
  }

  public comp() {
    this.compService.delete(this.data.id).subscribe(() => {
      if (this.removeTeams) {
        this.teamService.teams(this.data.id).subscribe(x => {
          this.teams = x;
          for (let t of this.teams)
            this.teamService.delete(t.id).subscribe((err: any) => { console.log(err); })
        }, (err: any) => { console.log(err); })
      }
      this.snack.delete('Competiton');
    }, (err: any) => {
      console.log(err);
      this.snack.xdelete('competiton');
    });
  }

  public team(): void {
    this.teamService.delete(this.data.id).subscribe(() => {
      this.snack.delete('Team');
    }, (err: any) => {
      console.log(err);
      this.snack.xdelete('team');
    })
  }

  public round(): void {
    this.roundService.delete(this.data.id).subscribe(() => {
      this.snack.delete('Round');
    }, (err: any) => {
      console.log(err);
      this.snack.xdelete('round');
    })
  }

  public game(): void {
    console.log('this game id is ' + this.data.id)
    this.gameService.delete(this.data.id).subscribe(() => {
      this.snack.delete('Game');
    }, (err: any) => {
      console.log(err);
      this.snack.xdelete('game');
    });
  }

  public gameRefresh(): void {
    this.data.refresh = true;
    this.cdr.markForCheck();
    //this.close();
  }

  public delete(): void {
    switch (this.data.type) {
      case 'c': this.comp();
        break;
      case 't': this.team();
        break;
      case 'r': this.round();
        break;
      case 'g': this.game();
        break;
    }
  }
}
