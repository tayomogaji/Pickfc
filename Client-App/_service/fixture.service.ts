import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Fixture } from '../_model/fixture';

type ClassName = 'win' | 'draw' | 'loose';
@Injectable({
  providedIn: 'root'
})
export class FixtureService {

  private readonly url: string = environment.baseUrl;

  constructor(private http: HttpClient) { }
  
  public fixture(id: number): Observable<Fixture> {
    return this.http.get<Fixture>(`${this.url}api/Fixture/Fixture?id=${id}`);
  }

  public teamFixture(teamid: number, roundid: number): Observable<Fixture> {
    return this.http.get<Fixture>(`${this.url}api/Fixture/TeamFixture?teamid=${teamid}&roundid=${roundid}`)
  }

  public fixtures(roundid: number): Observable<Fixture[]> {
    return this.http.get<Fixture[]>(`${this.url}api/Fixture/Fixtures?roundid=${roundid}`);
  }

  public addedit(fixture: Fixture): Observable<Fixture> {
    return fixture.id === 0 ? this.http.post<Fixture>(`${this.url}api/Fixture/Add`, fixture) : this.http.put<Fixture>(`${this.url}api/Fixture/Edit`, fixture);
  }

  public delete(id: number): Observable<Fixture> {
    return this.http.delete<Fixture>(`${this.url}api/Fixture/Delete?id=${id}`);
  }

  public hasFixture(teamid: number, roundid: number): Observable<boolean> {
    return this.http.get<boolean>(`${this.url}api/Fixture/HasFixture?teamid=${teamid}&roundid=${roundid}`);
  }

}
