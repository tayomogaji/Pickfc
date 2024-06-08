import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { FormService } from '../../_service/form.service';
import { GameService } from '../../_service/game.service';
import { SetService } from '../../_service/set.service';
import { SnackService } from '../../_service/snack.service';

@Component({
  selector: 'join',
  templateUrl: './join.component.html',
  styleUrls: ['../game.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class JoinComponent implements OnInit {

  game = this.setService.game();
  validCode: boolean = true;

  form = this.fb.group({
    code_input: this.fs.required(),
  });

  constructor(private cdr: ChangeDetectorRef, private router: Router, private fb: FormBuilder, private fs: FormService, private gameService: GameService, private setService: SetService, private snack: SnackService) { }

  ngOnInit(): void {}

  public close(): void {
    this.router.navigate(['']);
  }

  public join(): void {
    this.gameService.validCode(this.game.code).subscribe(x => {
      this.validCode = x;
      this.cdr.markForCheck();
    });

    if (this.validCode) {
      this.gameService.join(this.game.code).subscribe(x => {
        this.cdr.markForCheck();
        this.game = x;
        this.snack.join(x.name);
        this.router.navigate(['/game', this.game.id]);
      }, (err: any) => {
        console.log(err);
      });
    } else { this.snack.invalid() }
  }
}
