import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Art } from '../_model/art';

@Injectable({
  providedIn: 'root'
})
export class ArtService {

  private readonly url: string = environment.baseUrl;

  constructor(private http: HttpClient) { }

  public art(id: number): Observable<Art> {
    return this.http.get<Art>(`${this.url}api/Art/Art?id=${id}`);
  }

  public arts(): Observable<Art[]> {
    return this.http.get<Art[]>(`${this.url}api/Art/Arts`);
  }

  public addedit(art: Art): Observable<Art> {
    return art.id === 0 ? this.http.post<Art>(`${this.url}api/Art/Add`, art) : this.http.put<Art>(`${this.url}api/Art/Edit`, art);
  }

  public delete(id: number): Observable<Art> {
    return this.http.delete<Art>(`${this.url}api/Art/Delete?id=${id}`);
  }
}
