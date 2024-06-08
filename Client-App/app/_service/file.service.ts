import { HttpClient, HttpEventType } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as assert from 'assert';
import { retry } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FileService {

  private readonly url: string = environment.baseUrl;
  private readonly placeholder: string[] = ['/assets/placeholder/art.png', '/assets/placeholder/comp.png', '/assets/placeholder/team.png']

  constructor(private http: HttpClient) { }

  //public getFile(path: string) : string {
  //  return `${this.url}${path}`;
  //}
  public pic(pic: string, placeholderIndex: number): string {
    return pic === '' ? this.placeholder[placeholderIndex] : `${this.url}${pic}`
  }

  public upload(file: File, content: string, id: number) {
    if (file === null)
      return;

    const fd = new FormData();
    fd.append(file.name, file);

    this.http.post(`${this.url}api/Upload/${content}?id=${id}`, fd, { observe: 'events' })
      .subscribe(event => {
        if (event.type === HttpEventType.Response) {
          console.log('Success! - ' + file + ' uploaded');
        }
      }, (err: any) => {
        console.log(err);
      });
  }

  //public getPic(folderIndex: number, pic: string): string {
  //  switch (folderIndex) {
  //    case 0: this.path(0) + pic
  //      break;

  //  }
  //}

  //public getPic(img: Blob): string {
  //  var res = '';
  //  const reader = new FileReader();
  //  reader.readAsDataURL(img);
  //  reader.onload = () => {
  //    res = reader.result as string;
  //  };
  //  return res;
  //}

  //public getPicII(content: string, img: Blob, pic: any): string {
  //  switch (content) {
  //    case 'comp':
  //      this.setImg(img, pic)
  //      break;
  //    case 'game':
  //      pic === '' ? pic = this.placeholders[i] : pic = this.getFile(pic);
  //      break;
  //    case 'team':
  //      pic === '' ? pic = this.placeholders[i] : pic = this.getFile(pic);
  //      break;
  //    case 'art':
  //      pic === '' ? pic = this.placeholders[i] : pic = this.getFile(pic);
  //      break;
  //  }
  //  return pic;
  //}

  //public getPic(pic: string, i: number): string {
  //  switch (i) {
  //    case 0:
  //      pic = this.placeholders[i];
  //      break;
  //    case 1:
  //      pic === '' ? pic = this.placeholders[i] : pic = this.getFile(pic);
  //      break;
  //    case 2:
  //      pic === '' ? pic = this.placeholders[i] : pic = this.getFile(pic);
  //      break;
  //    case 3:
  //      pic === '' ? pic = this.placeholders[i] : pic = this.getFile(pic);
  //      break;
  //  }
  //  return pic;
  //}

  //image upload

}
