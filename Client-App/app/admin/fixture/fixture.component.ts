import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Fixture } from '../../_model/fixture';
import { Team } from '../../_model/team';
import { FileService } from '../../_service/file.service';
import { FixtureService } from '../../_service/fixture.service';
import { FormService } from '../../_service/form.service';
import { SetService } from '../../_service/set.service';
import { SnackService } from '../../_service/snack.service';
import { TeamService } from '../../_service/team.service';
type ClassName = 'win' | 'loose' | 'draw';

@Component({
  selector: 'app-fixture',
  templateUrl: './fixture.component.html',
  styleUrls: ['./fixture.component.css', '../admin.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class FixtureComponent implements OnInit {

  fixture = this.setService.fixture();
  fixturesCol: string[] = ['home_pic', 'home_name', 'home_res', 'vs', 'away_res', 'away_name', 'away_pic', 'edit_remove' ];
  fixturesData: any;
  fixtures = [this.setService.fixture()];

  results: string[] = ['W', 'L', 'D'];
  teams: Team[] = [this.setService.team()];
  teamIDs: number[] = [];

  pastDeadline: boolean = false;
  title: string = "";

  fixtureForm = this.fb.group({
    _home: this.fs.required(), _away: this.fs.required()
  });

  resultForm = this.fb.group({
    _result: this.fs.required()
  })

  constructor(private fb: FormBuilder, private fs: FormService, private cdr: ChangeDetectorRef, private fixtureService: FixtureService, private setService: SetService, private snack: SnackService, private fileServcie: FileService, public dialogRef: MatDialogRef<FixtureComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    this.getFixtures();
    this.deadlinePast();
  }

  public getFixture(id: number) {
    this.fixtureService.fixture(id).subscribe(x => {
      this.cdr.markForCheck();
      this.fixture = x;
      
    }, (err: any) => {
      console.log(err);
    })
    console.log(this.fixture.homeID + ' vs ' + this.fixture.awayID);
  }

  public getFixtures(): void {
    this.fixture.roundID = this.data.round.id;
    this.fixtureService.fixtures(this.data.round.id).subscribe(x => {
      this.fixturesData = new MatTableDataSource(x);
      this.fixtures = x;
      this.setTeams(x);
      this.cdr.markForCheck();
    }, (err: any) => { console.log(err); })
  }

  public pic(pic: string, ph: number): string {
    return this.fileServcie.pic(pic, ph);
  }

  public addedit(): void {
    if (!this.pastDeadline) {
      if (this.fixtureForm.valid) {
        this.fixtureModify(this.fixture);
        this.snack.add('Fixture')
      }
    } else
      for (let f of this.fixtures) {
        this.fixtureModify(f);
        this.snack.update('Result');
      }
    this.getFixtures();
  }

  public fixtureModify(fixture: Fixture): void {
    this.fixtureService.addedit(fixture).subscribe(() => {
      this.cdr.markForCheck();
      this.cdr.detectChanges();
    }, (err: any) => { console.log(err); });
    this.reset();
  }

  public setTeams(fs: Fixture[]): void {

    this.teamIDs.length = 0;
    this.teams.length = 0;

    for (let f of fs) {
      if (this.teamIDs.indexOf(f.homeID) === -1)
        this.teamIDs.push(f.homeID);

      if (this.teamIDs.indexOf(f.awayID) === -1)
        this.teamIDs.push(f.awayID);
    }

    for (let t of this.data.teams)
      if (this.teamIDs.indexOf(t.id) === -1)
        this.teams.push(t);
  }

  public homeSelect(e: any): void {
    this.fixture.homeID = e.value;
  }

  public awaySelect(e: any): void {
    this.fixture.awayID = e.value;
  }

  public resultSelect(f:Fixture, e: any): void {
    f.homeResult = e.value;
    switch (e.value) {
      case 'W':
        f.awayResult = 'L'
        break;
      case 'D':
        f.awayResult = 'D'
        break;
      case 'L':
        f.awayResult = 'W'
    }
  }

  public teamsInvalid = (): boolean => {
    const ti = this.fixture.homeID < 1 || this.fixture.awayID < 1 || this.fixture.homeID === this.fixture.awayID;
    return ti;
  }

  public reset(): void {
    this.fixture = this.setService.fixture();
  }

  public delete(id: number): void {
    this.fixtureService.delete(id).subscribe(() => {
      this.getFixtures();
      this.snack.delete('Fixture');
    }, (err: any) => {
      this.snack.xdelete('fixture');
      console.log(err);
    })
  }

  public deadlinePast(): void {
    this.pastDeadline = new Date() >= new Date(this.data.round.deadline);
    this.title = this.pastDeadline ? this.data.round.name + " - " + 'Results' : this.data.round.name + " - " + 'Fixtures';
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
}
