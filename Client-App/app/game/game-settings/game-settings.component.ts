import { ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Location } from '@angular/common';
import { EndOption } from '../../_model/end-option';
import { Player } from '../../_model/player';
import { FormBuilder } from '@angular/forms';
import { FileService } from '../../_service/file.service';
import { FormService } from '../../_service/form.service';
import { InfoService } from '../../_service/info.service';
import { SetService } from '../../_service/set.service';
import { GameService } from '../../_service/game.service';
import { Router } from '@angular/router';
import { SnackService } from '../../_service/snack.service';
import { PlayerService } from '../../_service/player.service';

@Component({
  selector: 'game-settings',
  templateUrl: './game-settings.component.html',
  styleUrls: ['./game-settings.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class GameSettingsComponent implements OnInit {

  @Input() game = this.setService.game();
  @Input() player = this.setService.player();
  @Input() players = [this.setService.player()];

  viewActive: boolean = false;
  viewAdmin: boolean = false;
  uploaded: boolean = false;

  file: File = new File([''], '');
  currentPic: any;

  endOptions: EndOption[] = [
    { action: 'Reset', desc: this.infoService.endGame[0], reset: true },
    { action: 'Delete', desc: this.infoService.endGame[1], reset: false }
  ]

  today = new Date();

  form = this.fb.group({
    _name: this.fs.required(), _dldate: this.fs.control()
  })

  constructor(private gameService: GameService, private playerService: PlayerService, private setService: SetService, private infoService: InfoService, private fileService: FileService, private fb: FormBuilder, private fs: FormService, private router: Router, private snack: SnackService, private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.currentPic = this.pic();
    this.sortPlayers();
  }

  public pic(): string {
    return this.fileService.pic(this.game.pic, 1);
  }

  public sortPlayers() {
    this.players.sort((a, b) => {
      if (a.name < b.name)
        return -1;

      if (a.name > b.name)
        return 1;

      return 0
    });
  }

  public deadlineIcon(): string {
    return this.game.deadline ? 'done' : 'horizontal_rule';
  }

  public code(): void {
    this.snack.copied('Game code');
  }

  public activeAdminIcon(player: Player, activeQury: boolean): string {
    if (activeQury)
      return player.active ? 'done' : 'close';
    else return player.admin ? 'done' : 'close';
  }

  public adminCreator(player: Player): boolean {
    if (player.id === this.player.id || player.user.id === this.game.creatorID)
      return true; else return false;
  }

  public endGame(reset: boolean): void {
    if (reset) {
      this.gameService.reset(this.game).subscribe(() => {
        this.snack.reset('Game');
        this.gameService.game(this.game.id).subscribe(() => {});
      }, (err: any) => {
        this.snack.xrestart('game');
        console.log(err);
      });
    }
    else {
      this.gameService.delete(this.game.id).subscribe(() => {
        this.router.navigate(['/my-games']);
        this.snack.delete('Game');
      }, (err: any) => {
        this.snack.xdelete('game');
        console.log(err);
      });
    }
  }

  public popTitle(player: Player, isActive: boolean): string {
    if (isActive)
      return player.active ? 'Deactivate ' + player.name + ', remove any admin premissions and eliminate them from the game?' : 'Reactivate ' + player.name + "'s player profile?";
    else
      return player.admin ? 'Remove admin access for ' + player.name : 'Grant admin access for ' + player.name + '?';
  }

  public popConfirm(player: Player, isActive: boolean): void {
    if (isActive)
      if (player.active) {
        player.active = false;
        player.admin = false
        player.eliminated = true;
      } else {
        player.active = true;
        if (player.life > 0)
          player.eliminated = false;
      }
    else
      player.admin ? player.admin = false : player.admin = true;

    this.playerService.edit(player).subscribe(() => {
      this.snack.update('Player');
      this.gameService.game(this.game.id).subscribe(() => { });
    }, (err: any) => {
      this.snack.xupdate('player');
      console.log(err);
    });
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

  public done(): void {
    if (this.game.name !== '') {
      this.gameService.addedit(this.game).subscribe(() => {
        this.snack.update('Game');
      }, (err: any) => {
        this.snack.xupdate('game');
        console.log(err);
      });
      if (this.uploaded) {
        this.fileService.upload(this.file, 'game', this.game.id);
        this.uploaded = false;
      };
    }
    this.gameService.game(this.game.id).subscribe(() => { }, (err: any) => {
      console.log(err);
    });
  }
}
