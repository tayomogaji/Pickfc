import { Component, Input, OnInit } from '@angular/core';
import { Player } from '../../_model/player';
import { FileService } from '../../_service/file.service';
import { PlayerService } from '../../_service/player.service';
import { SetService } from '../../_service/set.service';
type ClassName = 'win' | 'loose' | 'draw';

@Component({
  selector: 'profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css', ]
})
export class ProfileComponent implements OnInit {

  @Input() player = this.setService.player()
  @Input() players = [this.setService.player()]
  @Input() game = this.setService.game();
  @Input() deadlined: boolean = false;
  //'../game.component.css'

  @Input() playerPicks = [this.setService.pick()];

  maxLife: number = this.playerService.maxLife;
  lifeArr: number[] = [];

  constructor(private setService: SetService, private fileServcie: FileService, private playerService: PlayerService) { }

  ngOnInit(): void {
    this.viewSettings();
    this.setLife();
  }

  public viewSettings(): boolean {
    return this.player.id === this.game.currentPlayer.id && this.player.admin;
  }

  public champs(): number[] {
    const rateArr: number[] = [];
    for (let i = 0; i < this.player.champs; i++)
      rateArr.push(i)

    return rateArr;
  }

  public pic(pic: string, ph: number): string {
    return this.fileServcie.pic(pic, ph);
  }

  public artInfo(): string {
    var art = this.player.user.art;
    var i = '#' + art.index.toString() + ' ';
    return art.firstName === '' ? i + art.lastName.toLowerCase() : i + art.lastName.toLowerCase() + ', ' + art.firstName;
  }

  public nth(pos: number): string {
    var n = pos > 0
      ? ["th", "st", "nd", "rd"][
      (pos > 3 && pos < 21) || pos % 10 > 3 ? 0 : pos % 10
      ]
      : "";
    return pos.toString() + n;
  }

  public hitReveal(player: Player): string {
    var hr = ''
    if (player.hitByID > 0 && this.deadlined)
      hr = 'Hit by ' + player.hitByName;
    return hr;
  }

  public setLife(): void {
    for (let i = 0; i < this.maxLife; i++)
      this.lifeArr.push(i);
  }

  public lifeIcon(i: number): string {
    return this.player.life >= i + 1 ? 'favorite' : 'favorite_border';
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

  public chipPlay(played: number, total: number, playerid: number): string {
    var res = '? : ?';
    if (!this.deadlined && this.game.currentPlayer.id != playerid) {
      return res
    } else {
      return `${played} : ${total}`
    }
  }

  public pickTime(x: number): string {
    return x.toFixed(3)
  }
}
