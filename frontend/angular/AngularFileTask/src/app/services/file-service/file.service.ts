import { Injectable } from '@angular/core';
import { FileInfo } from 'src/app/contracts/file/file-info';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators'

@Injectable({
  providedIn: 'root'
})
export class FileService {

constructor(private httpClient: HttpClient) { }

  public loadRootDirectory() : Observable<FileInfo> {
    return this.httpClient.get<FileInfo>("/files/RootDirectory").pipe(map(FileInfo.createInstance));
  }

  public loadDirectory(directory : FileInfo) : Observable<FileInfo> {

    const config = { headers: new HttpHeaders().set('Content-Type', 'application/json') };

    return this.httpClient.post<FileInfo>("/files/GetDirectory", `"${directory.name.replace(/\\/g, "/")}"`, config).pipe(map(FileInfo.createInstance));
  }
}
