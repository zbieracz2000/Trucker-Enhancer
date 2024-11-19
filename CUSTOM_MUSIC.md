#Niestandardowa muzyka w grze

Muzyka w grze jest w formacie OGG Vorbis.
Aby patch na customową muzykę zadziałał, w folderze z grą w katalogu music musimy stworzyć podfolder ogg-custom.
Następnie w pliku !def.txt definujemy utwory które wrzuciliśmy do katalogu w takiej formie:

section Custom

track nazwapliku.ogg
name Nazwa utworu
artist Wykonawca

Linijkę "section Custom" dajemy TYLKO na górze pliku, by dodać kolejne utwory powtarzamy jedynie linijki track, name i artist.

