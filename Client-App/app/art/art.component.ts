import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Art } from '../_model/art';
import { ArtService } from '../_service/art.service';
import { FileService } from '../_service/file.service';
import { FormService } from '../_service/form.service';
import { SetService } from '../_service/set.service';
import { SnackService } from '../_service/snack.service';
import { UserService } from '../_service/user.service';

@Component({
  selector: 'art',
  templateUrl: './art.component.html',
  styleUrls: ['./art.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ArtComponent implements OnInit {

  art = this.data.user.art;
  arts = [this.setService.art()]
  viewList: boolean = false;
  viewAddedit: boolean = false;
  viewDelete: boolean = false
  uploaded: boolean = false;
  top: number = 10;

  currentPic: any;
  file: File = new File([''], '');

  form = this.fb.group({
    firstname: this.fs.control(), lastname: this.fs.required()
  });

  constructor(private artService: ArtService, private userService: UserService, private setService: SetService, private snack: SnackService, private fileService: FileService, private fs: FormService, private fb: FormBuilder, public dialogRef: MatDialogRef<ArtComponent>, @Inject(MAT_DIALOG_DATA) public data: any, private cdr: ChangeDetectorRef) { console.log(this.art.path)}

  ngOnInit(): void {
    if (this.data.user.id === 0)
      window.location.reload();
    this.getArts();
    this.viewSwitch(0);
  }

  public viewSwitch(i: number): void {
    switch (i) {
      case 0: this.viewList = true; this.viewAddedit = false; this.viewDelete = false;
        break;
      case 1: this.viewList = false; this.viewAddedit = true; this.viewDelete = false;
        break;
      case 2: this.viewList = false; this.viewAddedit = false; this.viewDelete = true;
    }
  }

  public upper(s: string): string {
    return s.toLocaleUpperCase();
  }

  public artSelect(art: Art): void {
    this.art = art;
    this.currentPic = this.pic(this.art.path, 0);
  }

  public getArts(): void {
    this.artSelect(this.data.user.art)
    this.artService.arts().subscribe(x => {
      this.arts = x
      this.cdr.markForCheck();
    }, (err: any) => { console.log(err); })
  }

  public addedit(): void {
    if (this.form.valid) {
      this.artService.addedit(this.art).subscribe(x => {
        this.upload('icon', x.id);
        this.cdr.markForCheck();
        this.art.id === 0 ? this.snack.add("Icon") : this.snack.update('Icon')
        this.artSelect(this.setService.art());
      }, (err: any) => {
        console.log(err);
        this.art.id === 0 ? this.snack.xadd('icon') : this.snack.xupdate('icon');
      })
    }
  }

  public delete(): void {
    this.artService.delete(this.art.id).subscribe(() => {
      this.getArts();
      this.listView();
      this.snack.delete('Icon');
    }, (err: any) => {
      console.log(err);
      this.snack.xdelete('icon');
    })
  }

  public userArt(id: number): void {
    this.data.user.artID = id;
    this.userService.addedit(this.data.user).subscribe(() => {
      this.snack.update('Icon');
      this.cdr.markForCheck();
    }, (err: any) => {
      console.log(err);
      this.snack.xupdate('icon');
    });
  }

  public set(): void {
    if (this.data.user.id > 0) {
      this.data.user.artID = this.art.id;
      this.userService.addedit(this.data.user).subscribe(() => {
        this.cdr.markForCheck();
        this.data.user.art = this.art;
        this.snack.update('Icon');
      }, (err: any) => {
        console.log(err);
        this.snack.xupdate('icon');
      });
    } else { window.location.reload(); }
  }

  public upload(content: string, id: number): void {
    if (this.uploaded) {
      this.fileService.upload(this.file, content, id);
      this.uploaded = false;
    };
  }

  public pic(pic: string, ph: number): string {
    return this.fileService.pic(pic, ph);
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

  public listView(): void {
    this.art = this.data.user.art;
    this.getArts();
    this.viewSwitch(0);
  }

  public addeditView(art: Art, newArt: boolean): void {
    newArt ? this.artSelect(this.setService.art()) : this.artSelect(art)
    this.viewSwitch(1)
  }

  public deleteView(art: Art): void {
    this.artSelect(art)
    this.viewSwitch(2);
  }

  public info(art: Art): string {
    var i = '#' + art.index.toString() + ' ';
    return art.firstName === '' ? i + art.lastName.toLowerCase() : i + art.lastName.toLowerCase() + ', ' + art.firstName;
  }

  public close(): void {
    this.dialogRef.close();
  }

}
