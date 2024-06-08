import { ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Game } from '../../_model/game';
import { CompService } from '../../_service/comp.service';
import { FormService } from '../../_service/form.service';
import { GameService } from '../../_service/game.service';
import { SetService } from '../../_service/set.service';
import { SnackService } from '../../_service/snack.service';
import { UserService } from '../../_service/user.service';
import { FileService } from '../../_service/file.service';
import { MatDialog } from '@angular/material/dialog';
import { UploadComponent } from '../../upload/upload.component';
import { DeleteComponent } from '../../delete/delete.component';

@Component({
  selector: 'addedit-game',
  templateUrl: './addedit-game.component.html',
  styleUrls: ['../game.component.css', '../game-settings/game-settings.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddEditGameComponent implements OnInit {

  comps = [this.setService.comp()];
  default = this.setService.comp();
  @Input() game = this.setService.game();
  @Input() games = [this.setService.game()];
  @Input() page: string = ''; 
  @Output() back = new EventEmitter<string>();

  id = this.ar.snapshot.params['id'];
  heading: string = '';

  admin: boolean = false;
  //newGame: boolean = true;
  refresh: boolean = false;

  today = new Date();

  form = this.fb.group({
    _name: this.fs.required(), _deadline: this.fs.control()
  });

  constructor(private ar: ActivatedRoute, private router: Router, private cdr: ChangeDetectorRef, private userService: UserService, private compService: CompService, private gameServcie: GameService, private setService: SetService, private fileService: FileService, private snack: SnackService, private fs: FormService, private fb: FormBuilder, public dialog: MatDialog) {

  }

  ngOnInit(): void {
    //this.addeditSet();
    this.getComps();
    this.getDefault();
  }

  public previous(): void {
    this.router.navigate(['']);
  }

  public pic(pic: string, ph: number): string {
    return this.fileService.pic(pic, ph);
  }

  public getComps(): void {
    this.compService.comps().subscribe(x => {
      this.comps = x;
      this.cdr.markForCheck();
    }, (err: any) => { console.log(err); })
  }

  public getGame(): void {
    this.gameServcie.game(this.game.id).subscribe(x => {
      this.game = x;
      this.cdr.markForCheck()
    }, (err: any) => { console.log(err); this.previous(); })
  }

  //public getPic(pic: string, i: number): string {
  //  return this.fileService.getPic(pic, i);
  //}

  public getDefault(): void {
    this.compService.default().subscribe(x => {
      if (this.game.compID === 0)
        this.game.compID = x.id;
      this.cdr.markForCheck();
    }, (err: any) => { console.log(err); })
  }

  public compSelect(e: any): void {
    this.game.compID = e.value;
  }

  public upload(): void {
    const id = this.game.id;
    //const pic = this.getPic(this.game.pic, 2);
    const name = this.game.name;
    const sig = 'Game';

    const dialogRef = this.dialog.open(UploadComponent, {
      autoFocus: false,
      data: {
        id: id, /*pic: pic,*/ name: name, sig: sig
      },
    });

    dialogRef.afterClosed().subscribe(() => { this.getGame(); });
  }

  public deadlineIcon(): string {
    return this.game.deadline ? 'done' : 'horizontal_rule';
  }

  public deadlineHint(): string {
    return this.game.deadline ? 'Players must join this game by...' : 'Players can join this game at any time.'
  }

  public delete(): void {
    const id: number = this.game.id;
    const name: string = this.game.name;
    const type: string = 'game';

    const dialogRef = this.dialog.open(DeleteComponent, {
      autoFocus: false,
      data: { id: id, name: name, type: type, refresh: this.refresh}
    });

    dialogRef.afterClosed().subscribe((err: any) => { console.log(err); })
  }

  public addedit(): void {
    if (this.form.valid) {
      this.gameServcie.addedit(this.game).subscribe(x => {
        this.cdr.markForCheck();
        this.snack.add('Game');
        this.router.navigate(['/my-games']);
      }, (err: any) => {
        console.log(err);
        this.snack.xadd('game');
      });
    }
  }

}
