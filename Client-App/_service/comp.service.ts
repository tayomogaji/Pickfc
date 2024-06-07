import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Comp } from '../_model/comp';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CompService {

  private readonly url: string = environment.baseUrl;

  constructor(private http: HttpClient) { }

  public comp(id: number): Observable<Comp> {
    return this.http.get<Comp>(`${this.url}api/Comp/Comp?id=${id}`);
  }

  public default(): Observable<Comp> {
    return this.http.get<Comp>(`${this.url}api/Comp/Default`);
  }

  public comps(): Observable<Comp[]> {
    return this.http.get<Comp[]>(`${this.url}api/Comp/Comps`);
  }

  public selection(teamid: number): Observable<Comp[]> {
    return this.http.get<Comp[]>(`${this.url}api/Comp/Selection?teamid=${teamid}`);
  }

  public hasTeam(id: number, teamid: number): Observable<boolean> {
    return this.http.get<boolean>(`${this.url}api/Comp/HasTeam?id=${id}&teamid=${teamid}`);
  }

  public defaultExist(): Observable<boolean> {
    return this.http.get<boolean>(`${this.url}api/Comp/DefaultExist`);
  }

  public addedit(comp: Comp): Observable<Comp> {
    return comp.id === 0 ? this.http.post<Comp>(`${this.url}api/Comp/Add`, comp) : this.http.put<Comp>(`${this.url}api/Comp/Edit`, comp);
  }

  public delete(id: number): Observable<Comp> {
    return this.http.delete<Comp>(`${this.url}api/Comp/Delete?id=${id}`);
  }

  public reset(comp: Comp): Observable<Comp> {
    return this.http.post<Comp>(`${this.url}api/Comp/Reset`, comp);
  }

  public defaultSwitch(id: number): Observable<Comp> {
    return this.http.put<Comp>(`${this.url}api/Comp/DefaultSwitch?id=${id}`, id);
  }


}
