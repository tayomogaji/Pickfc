import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, } from '@angular/material/dialog';
import { FileService } from '../_service/file.service';

@Component({
  selector: 'upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent {

  file: File = new File([''], '');
  uploaded: boolean = false;

  constructor(public dialogRef: MatDialogRef<UploadComponent>, @Inject(MAT_DIALOG_DATA) public data: any,
    private fileService: FileService){}

  public drag(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
  }

  public drop(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();

    const files = event.dataTransfer?.files;

    if (files && files.length > 0)
      this.droppedFiles(files);
  }

  public reader(x: any): void
  {
    var reader = new FileReader()
    reader.onload = (event: any) => {
      this.data.pic = event.target.result;
      this.uploaded = true;
    }
    reader.readAsDataURL(x);
  }

  public selectedFile(event: any): void
  {
    this.file = event.target.files[0]
    this.reader(this.file);
  }

  public droppedFiles(files: FileList)
  {
    this.file = files[0];
    this.reader(this.file);
  }

  public upload()
  {
    if (this.file === null)
      return;

    const fd = new FormData();
    fd.append('file', this.file, this.file.name);
    this.fileService.upload(this.file, this.data.content, this.data.id);
    this.close();
  }

  public close(): void {
    this.dialogRef.close();
  }
  
}
