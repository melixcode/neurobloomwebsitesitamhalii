/* ============================================================
   NeuroBloom — Uygulama Mantığı
   ============================================================== */
( fonksiyon ( ) {
' use strict ' ;
/* ---------- Depolama yardımcıları ---------- */
const KEY = ' neurobloom-v1 ' ;   
sabit varsayılanlar = {   
profil : { ad : ' ' , yaş : 7 , hedefMin : 10 , avatar : ' A ' } ,       
xp : 0 , seviye : 1 , seri : 0 , sonAktif : null ,     
bugünDakika : 0 , bugünTarih : null , günlükBitti : 0 ,    
egzersizlerTamamlandı : 0 ,  
aşamalar : { harfler : 0 , heceler : 0 , fiiller : 0 , sıfatlar : 0 , isimler : 0 , kelimeler : 0 , cümleler : 0 } ,          
duygular : [ ] , // {tarih, ruh hali, not}   
  geçmiş : [ ] , // günlük {tarih, doğru, toplam, dakika, ruh hali} 
  envanter : [ ] , donatılmış : { kıyafet : null , tema : null } ,    
premium : ' ücretsiz ' ,  
notlar : [ ] , // terapist notları   
  Ayarlar : { sfx : true , anim : true , remind : false , persona : ' Neşeli ' }     
} ;
let S = load ( ) ;   
fonksiyon yükle ( ) { 
try { const raw = localStorage.getItem ( KEY ) ; if ( ! raw ) return clone ( defaults ) ;​​         
const data = JSON.parse ( raw ) ; return Object.assign ( clone ( defaults ) , data ) ;​​​​          
} catch ( e ) { return clone ( defaults ) }     
}
function save ( ) { localStorage . setItem ( KEY , JSON . stringify ( S ) ) ; }    
fonksiyon klonla ( o ) { JSON'u ayrıştır ( JSON . stringify ( o ) ) döndür ; }​    
/* ---------- Tarih yardımcı programları ve günlük tohum ---------- */
fonksiyon bugün ( ) { yeni bir Date ( ) . toISOString ( ) . slice ( 0 , 10 ) ; döndür }     
fonksiyon ensureToday ( ) { 
sabit t = bugün ( ) ;     
eğer ( S.todayDate ! == t ) {​​​    
// Dün varsa onu geçmişe ekle    
    eğer ( S.todayDate ) {​​
S.history.push ( { date : S.todayDate , correct : S._tcorrect || 0 , total : S._ttotal || 0 , minutes : S.todayMinutes || 0 , mood : S._tmood || ' neutral ' } ) ;​​​​​​​​​​​​​​​​​​            
while ( S . history . length > 30 ) S . history . shift ( ) ;       
}    
S.todayDate = t ; S.todayMinutes = 0 ; S.dailyDone = 0 ;​​​​​​            
S . _tcorrect = 0 ; S . _ttotal = 0 ; S . _tmood = ' nötr ' ;      
// seri mantığı    
    eğer ( S.lastActive ) {​​
const diff = ( new Date ( t ) - new Date ( S . lastActive ) ) / 86400000 ;           
eğer ( fark > 1 ) S.streak = 0 ;​​         
}    
}  
// Boşsa geçmişe dair bazı veriler ekleyin (güzel grafikler için)  
  Eğer ( S.history.length < 6 ) ise {​​​
sabit tohum = [ ' mutlu ' , ' nötr ' , ' heyecanlı ' , ' yorgun ' , ' mutlu ' , ' nötr ' , ' heyecanlı ' ] ;     
for ( let i = 6 ; i > = 1 & & S . history . length < 6 ; i -- ) {​       
const d = new Date ( ) ; d.setDate ( d.getDate ( ) - i ) ;​​​​         
S.history.unshift ( { date : d.toISOString ( ) . slice ( 0 , 10 ) , correct : 6 + Math.floor ( Math.random ( ) * 8 ) , total : 10 + Math.floor ( Math.random ( ) * 4 ) , minutes : 6 + Math.floor ( Math.random ( ) * 14 ) , mood : seed [ i % seed.length ] } ) ;​​​​​​​​​​​​​​​​​​​​          
}    
}  
kaydet ( ) ;  
}
/* ---------- DOM kullanımları ---------- */
const $ = ( s , el = belge ) = > el . querySelector ( s ) ;   
const $$ = ( s , el = document ) = > Array . from ( el . querySelectorAll ( s ) ) ;   
fonksiyon göster ( id ) { [ ' açılış sayfası ' , ' hoş geldiniz ' , ' uygulama ' , ' üst öğe ' ] . forEach ( x = > {  
const el = document.getElementById ( x ) ; if ( ! el ) return ;​​     
eğer ( x === ' welcome ' ) { el.classList.toggle ( ' active ' , id === ' welcome ' ) ; }​​​​​​​​     
aksi takdirde eğer ( x === ' landing ' ) { el.style.display = id === ' landing ' ? ' block ' : ' none ' ; }​​​​​​​       
aksi takdirde eğer ( x === ' app ' ) { el.classList.toggle ( ' active ' , id === ' app ' ) ; }​​​​​​​      
aksi takdirde eğer ( x === ' parent ' ) { el.classList.toggle ( ' active ' , id === ' parent ' ) ; }​​​​​​​      
} ) ; }
/* ---------- NeuroBot SVG ---------- */
fonksiyon botSVG ( boyut = 160 ) { 
return ` <svg viewBox="0 0 200 200" width=" ${ size } " height=" ${ size } " xmlns="http://www.w3.org/2000/svg">   
    <def>
      <linearGradient id="bg" x1="0" y1="0" x2="1" y2="1">
        <stop offset="0%" stop-color="#AFCBFF"/><stop offset="100%" stop-color="#D8C2FF"/>
      </linearGradient>
      <linearGradient id="bd" x1="0" y1="0" x2="0" y2="1">
        <stop offset="0%" stop-color="#FFFFFF"/><stop offset="100%" stop-color="#E9EEFF"/>
      </linearGradient>
    </defs>
    <circle cx="100" cy="100" r="92" fill="url(#bg)" opacity=".35"/>
    <rect x="42" y="60" width="116" height="100" rx="34" fill="url(#bd)" stroke="#C9D3F0" stroke-width="2"/>
    <rect x="86" y="40" width="28" height="22" rx="6" fill="#C8E3FF" stroke="#9FB5E8" stroke-width="2"/>
    <circle cx="100" cy="36" r="6" fill="#FFD36E" stroke="#E0A93C" stroke-width="2"/>
    <circle cx="78" cy="100" r="10" fill="#1B2440"/><circle cx="122" cy="100" r="10" fill="#1B2440"/>
    <circle cx="81" cy="97" r="3" fill="#fff"/><circle cx="125" cy="97" r="3" fill="#fff"/>
    <circle cx="65" cy="120" r="8" fill="#FFB7C2" opacity=".7"/>
    <circle cx="135" cy="120" r="8" fill="#FFB7C2" opacity=".7"/>
    <path d="M82 130 Q100 144 118 130" stroke="#1B2440" stroke-width="4" fill="none" stroke-linecap="round"/>
    <rect x="58" y="160" width="28" height="18" rx="6" fill="#9F8CFF"/>
    <rect x="114" y="160" width="28" height="18" rx="6" fill="#9F8CFF"/>
  </svg> ` ;
}
[ ' heroBot ' , ' welcomeBot ' , ' homeBot ' , ' roomBot ' ] . forEach ( id = > {
const el = belge . getElementById ( kimlik ) ; if ( el ) el . innerHTML = botSVG ( id = = = ' heroBot ' ? 240 : id = = = ' hoşgeldinBot ' ? 160 : 160 ) ;       
} ) ;
/* ---------- Açılış → Hoş Geldiniz → Mod ---------- */
document.getElementById ( ' yr ' ) . textContent = new Date ( ) . getFullYear ( ) ;​​   
$$ ( ' [data-cta= " enter " ] ' ) . forEach ( b = > b . addEventListener ( ' click ' , ( ) = > show ( ' welcome ' ) ) ) ;   
$$ ( ' .mode-pick ' ) . forEach ( b = > b . addEventListener ( ' click ' , ( ) = > {  
sabit m = b.veri kümesi.mod ;​​​     
eğer ( m === ' çocuk ' ise , çocuğu aç ; aksi halde ebeveyni aç ;​​     
} ) ) ;
$$ ( ' [veri-planı] ' ) . forEach ( b = > b . addEventListener ( ' click ' , e = > openPay ( e . currentTarget . dataset . plan ) ) ) ;   
function openChild ( ) { ensureToday ( ) ; show ( ' app ' ) ; navTo ( ' home ' ) ; renderAll ( ) ; }      
function openParent ( ) { ensureToday ( ) ; show ( ' parent ' ) ; renderParent ( ) ; }     
/* ---------- Gezinme ---------- */
$$ ( ' #bottomNav button ' ) . forEach ( b = > b . addEventListener ( ' click ' , ( ) = > navTo ( b . dataset . nav ) ) ) ;   
fonksiyon navTo ( isim ) { 
  $$ ( ' #bottomNav button ' ) . forEach ( x = > x . classList . toggle ( ' active ' , x . dataset . nav = = = name ) ) ;  
  $$ ( ' #app .screen ' ) . forEach ( s = > s . classList . toggle ( ' active ' , s . id = = = ' screen- ' + name ) ) ;  
eğer ( isim = = = ' analitik ' ) drawCharts ( ) ;   
eğer ( isim === ' oda ' ise ) renderRoom ( ) ;​   
eğer ( isim === ' profil ' ) renderProfile ( ) ;​​   
eğer ( isim === ' premium ' ise ) renderPremium ( ) ;​   
}
$$ ( ' [data-go] ' ) . forEach ( b = > b . addEventListener ( ' click ' , ( ) = > {  
sabit t = b.dataset.go ;​​​​     
eğer ( t === ' practice ' ) { navTo ( ' home ' ) ; document.getElementById ( ' practice ' ) . scrollIntoView ( { behavior : ' smooth ' } ) ; }​​​​     
aksi takdirde eğer ( t === ' games ' ) { document.getElementById ( ' games ' ) . scrollIntoView ( { behavior : ' smooth ' } ) ; }​​​     
} ) ) ;
/* ---------- XP / Seviye ---------- */
fonksiyon addXP ( n , sebep = ' ' ) {  
S . xp + = n ;    
sabit ihtiyaç = ( seviye ) = > 80 + seviye * 40 ;        
while ( S . xp > = need ( S . level ) ) { S . xp - = need ( S . level ) ; S . level + + ; toast ( ' 🎉 Seviye Atladınız! Artık Seviyeniz ' + S . level ) ; confetti ( ) ; }           
S . dailyDone + + ; S . _ttotal = ( S . _ttotal | | 0 ) + 1 ; S . _tcorrect = ( S . _tcorrect | | 0 ) + 1 ;    
S . egzersizler tamamlandı + + ; S . son aktif = bugün ( ) ;     
// seri — günün ilk antrenmanının ödülü  
  Eğer ( S.dailyDone === 1 ) ise , S.streak = ( S.streak || 0 ) + 1 ; }​​​​​​​​      
kaydet ( ) ; renderHeader ( ) ; renderHomeKPIs ( ) ;    
eğer ( n > 0 ) xpPop ( ' + ' + n + ' XP ' + ( sebep ? ' · ' + sebep : ' ' ) ) ;   
}
fonksiyon xpPop ( metin ) { 
const el = document.createElement ( ' div ' ) ; el.className = ' xp - pop ' ; el.textContent = text ;​​​​       
document.body.appendChild ( el ) ; setTimeout ( ( ) = > el.remove ( ) , 900 ) ;​​​​​​    
}
/* ---------- Tost ---------- */
function toast ( msg ) { const el = document . createElement ( ' div ' ) ; el . className = ' toast ' ; el . textContent = msg ; document . body . appendChild ( el ) ; setTimeout ( ( ) = > el . remove ( ) , 2400 ) ; }        
/* ---------- Konfeti ---------- */
const conCanvas = belge . getElementById ( ' konfeti ' ) ;   
const conCtx = conCanvas.getContext ( ' 2d ' ) ;​​   
function resizeConfetti ( ) { conCanvas . width = innerWidth ; conCanvas . height = innerHeight ; }        
addEventListener ( ' resize ' , resizeConfetti ) ; resizeConfetti ( ) ;  
let conParts = [ ] ; let conRAF = null ;     
fonksiyon konfeti ( ) { 
sabit renkler = [ ' #AFCBFF ' , ' #D8C2FF ' , ' #A7E8D6 ' , ' #FFD36E ' , ' #9F8CFF ' , ' #7DA8FF ' ] ;     
for ( let i = 0 ; i < 140 ; ​​i + + ) conParts . push ( { x : innerWidth / 2 , y : innerHeight / 3 , vx : ( Math . random ( ) - . 5 ) * 8 , vy : Math . random ( ) * - 9 - 2 , g : . 25 , c : colors [ i % colors . length ] , s : 4 + Math . random ( ) * 4 , r : Math . random ( ) * Math . PI , vr : ( Math . random ( ) - . 5 ) * . 3 , life : 120 } ) ;    
eğer ( ! conRAF ) conRAF = requestAnimationFrame ( conTick ) ;     
}
fonksiyon conTick ( ) { 
conCtx . clearRect ( 0 , 0 , conCanvas.genişlik , conCanvas.yükseklik ) ;​​​​  
conParts.forEach ( p = > { p.vy + = p.g ; p.x + = p.vx ; p.y + = p.vy ; p.r + = p.vr ; p.life -- ; conCtx.save ( ) ; conCtx.translate ( p.x , p.y ) ; conCtx.rotate ( p.r ) ; conCtx.fillStyle = p.c ; conCtx.fillRect ( -p.s / 2 , -p.s / 2 , p.s , p.s * 1.4 ) ; conCtx.restore ( ) ; } ) ;​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​              
conParts = conParts . filter ( p = > p . life > 0 & & p . y < conCanvas . height + 40 ) ;      
if ( conParts.length ) { conRAF = requestAnimationFrame ( conTick ) ;​​ } else { conRAF = null ; conCtx . clearRect ( 0 , 0 , conCanvas.genişlik , conCanvas.yükseklik ) ;​​​​ }           
}
/* ---------- Başlık / KPI'lar ---------- */
fonksiyon renderHeader ( ) { 
  $ ( ' #streakPill ' ) . textContent = S . streak ;  
  $ ( ' #xpPill ' ) . textContent = S . xp + ( S . level > 1 ? ' · L ' + S . level : ' ' ) ;    
  $ ( ' #avatarInit ' ) . textContent = ( S . profile . name | | ' A ' ) . slice ( 0 , 1 ) . toUpperCase ( ) ;  
eğer ( S.profil.adı ) $ ( ' # greetName ' ) . textContent = ' Merhaba ' + S.profil.adı + ' ! ' ;​​​​​​​     
}
fonksiyon renderHomeKPIs ( ) { 
sabit ihtiyaç = 80 + S.seviye * 40 ;​​       
  $ ( ' #kpiXP ' ) . metinİçerik = S.​ xp ;  
  $ ( ' #kpiXPBar ' ) . style . width = Math . min ( 100 , ( S . xp / need ) * 100 ) + ' % ' ;  
  $ ( ' #kpiLvl ' ) . metinİçerik = S.​ seviye ;  
  $ ( ' #kpiStreak ' ) . textContent = S . streak + ' 🔥 ' ;  
  $ ( ' #kpiGoal ' ) . textContent = S . dailyDone ;  
  $ ( ' #kpiGoalBar ' ) . style . width = Math . min ( 100 , ( S . dailyDone / 5 ) * 100 ) + ' % ' ;  
  $ ( ' #kpiBugün ' ) . textContent = ( S. TodayMinutes | | 0 ) + ' min ' ;​  
}
/* ---------- Konuşma Yolu ---------- */
sabit AŞAMALAR = [   
{ id : ' harfler ' , label : ' Harfler ' , need : 0 } ,      
{ id : ' heceler ' , label : ' Heceler ' , ihtiyaç : 30 } ,      
{ id : ' fiiller ' , label : ' Fiiller ' , need : 80 } ,      
{ id : ' sıfatlar ' , label : ' Sıfatlar ' , need : 140 } ,      
{ id : ' isimler ' , etiket : ' İsimler ' , ihtiyaç : 200 } ,      
{ id : ' kelimeler ' , label : ' Kelimeler ' , need : 280 } ,      
{ id : ' cümleler ' , label : ' Cümleler ' , need : 360 } ,      
] ;
fonksiyon renderPath ( ) { 
const el = $ ( ' #konuşmaYolu ' ) ; el . içHTML = ' ' ;      
const totalXP = S . xp + ( S . level - 1 ) * 200 ; // yaklaşık ömür        
  AŞAMALAR . forEach ( ( st , i ) = > {
const node = document.createElement ( ' div ' ) ;​​       
node.className = ' node ' ;​​      
sabit kilidi açılmış = toplamXP > = st.need ;​         
eğer ( ! kilit açık ise ) düğüm . sınıf listesine ' kilitli ' ekle ;     
sabit prog = S.stages [ st.id ] || 0 ;​​​​​       
eğer ( prog > = 5 ) node.classList.add ( ' done ' ) ;​​​​     
eğer ( i === STAGES.findIndex ( x = > ( S.stages [ x.id ] || 0 ) < 5 && totalXP > = x.need ) ) node.classList.add ( ' current ' ) ;​​​​​​​​​​​​​​​​        
node . innerHTML = ` <div> ${ st . label } </div><small> ${ unlocked ? prog + ' /5 ' : ' Unlock at ' + st . need + ' XP ' } </small> ` ;      
eğer ( kilitsiz ) düğüm.addEventListener ( ' click ' , ( ) = > openExercise ( st.id ) ) ;​​​​       
el . appendChild ( node ) ;    
} ) ;  
}
/* ---------- Alıştırmalar ---------- */
sabit BANKA = {   
harfler : [ ' A ' , ' B ' , ' C ' , ' D ' , ' E ' , ' F ' , ' G ' , ' M ' , ' O ' , ' R ' , ' S ' , ' T ' ] ,   
heceler : [ ' BA ' , ' BE ' , ' MA ' , ' ME ' , ' SU ' , ' SO ' , ' RA ' , ' RE ' , ' LI ' , ' LU ' , ' PA ' , ' PE ' , ' TA ' , ' TI ' ] ,   
fiiller : [ ' koşmak ' , ' zıplamak ' , ' okumak ' , ' şarkı söylemek ' , ' yüzmek ' , ' oynamak ' , ' çizmek ' , ' dans etmek ' , ' uyumak ' , ' yemek ' ] ,   
sıfatlar : [ ' mutlu ' , ' küçük ' , ' yumuşak ' , ' parlak ' , ' sessiz ' , ' sıcak ' , ' serin ' , ' cesur ' , ' nazik ' , ' komik ' ] ,   
isimler : [ ' elma ' , ' nehir ' , ' bulut ' , ' kitap ' , ' ev ' , ' yıldız ' , ' çiçek ' , ' tren ' , ' panda ' , ' ay ' ] ,   
kelimeler : [ ' kelebek ' , ' gökkuşağı ' , ' yunus ' , ' güneş ışığı ' , ' kumdan kale ' , ' kütüphane ' , ' şemsiye ' ] ,   
cümleler : [ ' kedi uyuyor ' , ' ailemi seviyorum ' , ' parkta oynuyoruz ' , ' güneş parlak ' , ' odam düzenli ' ]   
} ;
sabit İPUÇLARI = {   
harfler : ' Harfin sesini söyle, sonra yaz . '  
heceler : ' Heceyi yavaşça söyleyin: ağzınızı tamamen açın . '  
fiiller : ' Eylem kelimesini söyleyin. Mümkünse mimiklerle gösterin ! '  
sıfatlar : ' Tanımlayıcı kelimeyi duyguyla söyleyin . '  
isimler : ' Nesnenin adını yüksek sesle söyleyin, sonra yazın . '  
kelimeler : ' Hecelere ayırın, sonra birlikte söyleyin . '  
cümleler : ' Yüksek sesle, açık ve gururla okuyun. '  
} ;
let exCtx = { stage : null , target : ' ' , } ;       
fonksiyon openExercise ( aşama ) { 
exCtx . aşama = aşama ;    
pickExerciseTarget ( ) ;  
  $ ( ' #exTitle ' ) . textContent = ' Uygulama: ' + stage . charAt ( 0 ) . toUpperCase ( ) + stage . slice ( 1 ) ;   
  $ ( ' #exTip ' ) . textContent = TIPS [ stage ] ;  
// isteğe bağlı ekstralar: ağız yönlendirmesi + harfler/heceler için dudak/dil hareketi  
  const extra = $ ( ' #exExtra ' ) ;   
eğer ( aşama = == ' harf ' | | aşama = == ' hece ' ) {​​    
extra . innerHTML = ` <div class="mouth" title="Visual mouth guidance"></div>      
      <div style="display:flex;gap:8px;justify-content:center;margin:6px 0 4px;flex-wrap:wrap">
        <span class="pill">👄 Dudak egzersizi: dudaklarınızı yuvarlaklaştırın</span>
        <span class="pill">👅 Dil: ucu dişlere değiyor</span>
        <span class="pill">🔁 3 defa tekrarlayın</span>
      </div> ` ;
} başka ekstra . içHTML = ' ' ;      
  $ ( ' #exInput ' ) . value = ' ' ;  
  $ ( ' #exModal ' ) . classList . add ( ' active ' ) ;
setTimeout ( ( ) = > $ ( ' #exInput ' ) . focus ( ) , 100 ) ;   
trackTime ( ) ;  
}
fonksiyon pickExerciseTarget ( ) { 
sabit liste = BANK [ exCtx . aşama ] | | BANK . harfler ;       
exCtx.target = list [ Math.floor ( Math.random ( ) * list.length ) ] ;​​​​​​​​    
  $ ( ' #exTarget ' ) . textContent = exCtx . hedef ;  
}
$ ( ' #exCheck ' ) . addEventListener ( ' click ' , ( ) = > { 
const val = ($ ( ' #exInput ' ) . value | | ' ' ) . trim ( ) . toLowerCase ( ) ;     
const ok = val & & val = = = String ( exCtx . target ) . toLowerCase ( ) ;         
S . _ttotal = ( S . _ttotal | | 0 ) + 1 ;    
eğer ( tamam ) {  
addXP ( 15 , ' pratik ' ) ;    
S . aşamalar [ exCtx . aşama ] = Math . min ( 5 , ( S . aşamalar [ exCtx . aşama ] | | 0 ) + 1 ) ;      
tost ( ' ✅ Harika iş! +15 XP ' ) ;    
pickExerciseTarget ( ) ; $ ( ' #exInput ' ) . value = ' ' ;     
renderPath ( ) ;    
} aksi halde {    
S . _ttotal = S . _ttotal ; // deneme sayısını say       
    tost ( ' Tekrar dene — dinle ve tekrarla 🌱 ' ) ;
    $ ( ' #exInput ' ) . style . borderColor = ' #ff8a8a ' ;  
setTimeout ( ( ) = > $ ( ' #exInput ' ) . style . borderColor = ' ' , 400 ) ;     
}  
kaydet ( ) ;  
} ) ;
$ ( ' #exSkip ' ) . addEventListener ( ' click ' , ( ) = > { pickExerciseTarget ( ) ; $ ( ' #exInput ' ) . value = ' ' ; } ) ;    
$ ( ' #exHint ' ) . addEventListener ( ' click ' , ( ) = > { 
const t = String ( exCtx . target ) ;     
toast ( ' İpucu: “ ' + t . charAt ( 0 ) . toUpperCase ( ) + ' ” · ' + t . length + ' harfle başlar ' ) ;  
} ) ;
$$ ( ' [data-close] ' ) . forEach ( b = > b . addEventListener ( ' click ' , ( ) = > b . closest ( ' .modal ' ) . classList . remove ( ' active ' ) ) ) ;   
/* ---------- Duygu + mikrofon ---------- */
let pickedEmotion = null ;   
$$ ( ' #duygular düğmesi ' ) . forEach ( b = > b . addEventListener ( ' tıklama ' , ( ) = > {  
  $$ ( ' #emotions button ' ) . forEach ( x = > x . classList . remove ( ' sel ' ) ) ; 
b.classList.add ( ' sel ' ) ; pickedEmotion = b.dataset.emo ; S._tmood = pickedEmotion ;​​​​​​​​​​        
} ) ) ;
$ ( ' #saveEmo ' ) . addEventListener ( ' click ' , ( ) = > { 
eğer ( ! pickedEmotion ) { toast ( ' Önce bir duygu seçin 🙂 ' ) ; return ; }     
S . duygular . itme ( { tarih : bugün ( ) , ruh hali : seçilenDuygu , not : $ ( ' #emoNot ' ) . değer | | ' ' } ) ;         
  $ ( ' #emoNote ' ) . value = ' ' ; pickedEmotion = null ; $$ ( ' #emotions button ' ) . forEach ( x = > x . classList . remove ( ' sel ' ) ) ;  
addXP ( 5 , ' check-in ' ) ; toast ( ' Kaydedildi 💗 ' ) ;   
} ) ;
let mediaRec = null , micActive = false , micRAF = null ;   
$ ( ' #micBtn ' ) . addEventListener ( ' click ' , async ( ) = > {  
eğer ( mikrofon aktifse ) { mikrofonu durdur ; geri dön ; }     
dene {  
const stream = await navigator.mediaDevices.getUserMedia ( { audio : true } ) ;​​​​        
const ctx = new ( window . AudioContext | | window . webkitAudioContext ) ( ) ;        
const src = ctx . createMediaStreamSource ( stream ) ;       
const an = ctx.createAnalyser ( ) ; an.fftSize = 256 ;​​​​          
src . connect ( an ) ;    
const buf = new Uint8Array ( an . frequencyBinCount ) ;        
mediaRec = { stream , ctx } ;         
micActive = true ; $ ( ' #micBtn ' ) . classList . add ( ' rec ' ) ;     
sabit tick = ( ) = > {       
bir . getByteFrequencyData ( buf ) ;      
let sum = 0 ; for ( let i = 0 ; i < buf . length ; i + + ) sum + = buf [ i ] ;          
sabit seviye = Math.min ( 100 , ( toplam / tampon.uzunluk ) * 1.2 ) ;​​​​​​         
      $ ( ' #micMeter ' ) . style . width = lvl + ' % ' ;  
eğer ( mikrofon aktifse ) micRAF = requestAnimationFrame ( tick ) ;         
} ; tik ( ) ;     
} catch ( e ) { toast ( ' Mikrofon izni reddedildi — simüle edilmiş geri bildirim kullanılıyor ' ) ; simulateMic ( ) ; }     
} ) ;
fonksiyon stopMic ( ) { 
micActive = false ; cancelAnimationFrame ( micRAF ) ; $ ( ' #micBtn ' ) . classList . remove ( ' rec ' ) ;    
  $ ( ' #micMeter ' ) . style . width = ' 0% ' ;  
eğer ( mediaRec ) { try { mediaRec.stream.getTracks ( ) . forEach ( t = > t.stop ( ) ) ; mediaRec.ctx.close ( ) ; } catch ( e ) { } mediaRec = null ; }​​​​​​​​​​        
toast ( ' 🎧 Kayıt kaydedildi ' ) ;  
}
fonksiyon simulateMic ( ) { 
micActive = true ; $ ( ' #micBtn ' ) . classList . add ( ' rec ' ) ;   
let t = 0 ; const tick = ( ) = > { t + = 1 ; $ ( ' #micMeter ' ) . style . width = ( 40 + Math . sin ( t / 4 ) * 30 + Math . random ( ) * 20 ) + ' % ' ; if ( micActive ) micRAF = requestAnimationFrame ( tick ) ; } ; tick ( ) ;             
setTimeout ( stopMic , 3000 ) ;   
}
/* ---------- Profil ---------- */
fonksiyon renderProfile ( ) { 
  $ ( ' #profileName ' ) . value = S . profile . name | | ' ' ;  
  $ ( ' #profileAge ' ) . value = S . profile . age | | ' ' ;  
  $ ( ' #profileGoal ' ) . value = S . profile . goalMin | | 10 ;  
  $ ( ' #profileAvatar ' ) . textContent = ( S . profile . name | | ' A ' ) . slice ( 0 , 1 ) . toUpperCase ( ) ;  
  $ ( ' #pXP ' ) . textContent = S . xp ; $ ( ' #pLvl ' ) . textContent = S . level ;     
  $ ( ' #pStreak ' ) . textContent = S . streak + ' 🔥 ' ; $ ( ' #pEx ' ) . textContent = S . exercisesDone ;     
  $ ( ' #pPlan ' ) . textContent = S . premium = = = ' free ' ? ' Free ' : S . premium = = = ' starter ' ? ' Starter ' : ' Therapist ' ;  
sabit ach = [     
{ id : ' first ' , name : ' İlk Adım ' , icon : ' 🌱 ' , cond : S . exercisesDone > = 1 } ,        
{ id : ' ten ' , name : ' Ten Strong ' , icon : ' 💪 ' , cond : S . exercisesDone > = 10 } ,        
{ id : ' streak3 ' , name : ' 3 Günlük Seri ' , icon : ' 🔥 ' , cond : S . streak > = 3 } ,        
{ id : ' level3 ' , name : ' Seviye 3 ' , icon : ' ⭐ ' , cond : S . level > = 3 } ,        
{ id : ' gameWin ' , name : ' Oyun Başladı! ' , icon : ' 🎮 ' , cond : ( S . inventory . includes ( ' gamewin ' ) ) } ,        
{ id : ' rich ' , name : ' XP Tasarrufu ' , icon : ' 💎 ' , cond : S . xp > = 200 }        
] ;  
  $ ( ' #achievements ' ) . innerHTML = ach . map ( a = > ` <div class="glass" style="padding:14px;border-radius:14px;display:flex;gap:10px;align-items:center;opacity: ${ a . cond ? 1 : . 5 } ">  
    <div style="width:36px;height:36px;border-radius:12px;background:var(--grad-primary);display:grid;place-items:center"> ${ a . icon } </div>
    <div><b> ${ a . name } </b><div style="font-size:.78rem;color:var(--c-muted)"> ${ a . cond ? ' Unlocked ' : ' Locked ' } </div></div></div> ` ) . join ( ' ' ) ;
}
$ ( ' #saveProfile ' ) . addEventListener ( ' click ' , ( ) = > { 
S . profil . adı = $ ( ' #profilAdı ' ) . değer . kırp ( ) ;    
S . profil . yaş = +$ ( ' #profilYaşı ' ) . değer | | 7 ;    
S . profil . hedefMinimum = +$ ( ' #profilHedefi ' ) . değer | | 10 ;    
kaydet ( ) ; renderHeader ( ) ; renderProfile ( ) ; toast ( ' Profil kaydedildi ' ) ;     
} ) ;
/* ---------- Oda + Dükkan ---------- */
sabit MAĞAZA = [   
{ id : ' yatak ' , cat : ' mobilya ' , name : ' Rahat Yatak ' , cost : 40 , ico : ' 🛏️ ' , x : ' 8% ' , y : ' 30% ' } ,        
{ id : ' lamba ' , cat : ' mobilya ' , name : ' Yumuşak Lamba ' , cost : 30 , ico : ' 💡 ' , x : ' 14% ' , y : ' 8% ' } ,        
{ id : ' raf ' , cat : ' mobilya ' , name : ' Kitaplık ' , cost : 80 , ico : ' 📚 ' , x : ' 72% ' , y : ' 20% ' } ,        
{ id : ' masa ' , cat : ' mobilya ' , name : ' Çalışma Masası ' , cost : 120 , ico : ' 🪑 ' , x : ' 68% ' , y : ' 45% ' } ,        
{ id : ' bitki ' , cat : ' dekorasyon ' , name : ' Mutlu Bitki ' , cost : 25 , ico : ' 🪴 ' , x : ' 22% ' , y : ' 55% ' } ,        
{ id : ' poster ' , cat : ' dekorasyon ' , name : ' Yıldız Poster ' , cost : 35 , ico : ' 🖼️ ' , x : ' 42% ' , y : ' 12% ' } ,        
{ id : ' pencere ' , cat : ' dekorasyon ' , name : ' Güneşli Pencere ' , cost : 60 , ico : ' 🪟 ' , x : ' 82% ' , y : ' 8% ' } ,        
{ id : ' halı ' , cat : ' dekorasyon ' , name : ' Gökkuşağı Halı ' , cost : 50 , ico : ' 🟣 ' , x : ' 40% ' , y : ' 70% ' } ,        
{ id : ' pelerin ' , cat : ' kıyafet ' , name : ' Kahraman Pelerini ' , cost : 120 , ico : ' 🦸 ' , equip : ' kıyafet ' } ,       
{ id : ' eşarp ' , cat : ' kıyafet ' , name : ' Rahat Eşarp ' , cost : 80 , ico : ' 🧣 ' , equip : ' kıyafet ' } ,       
{ id : ' şapka ' , cat : ' aksesuar ' , name : ' Güneş Şapkası ' , cost : 60 , ico : ' 🎩 ' , x : ' 48% ' , y : ' 56% ' } ,        
{ id : ' gözlük ' , cat : ' aksesuar ' , name : ' Akıllı Gözlük ' , cost : 90 , ico : ' 🤓 ' , x : ' 50% ' , y : ' 58% ' } ,        
{ id : ' theme-pastel ' , cat : ' theme ' , name : ' Pastel Tema ' , cost : 100 , ico : ' 🌸 ' , equip : ' theme ' } ,       
{ id : ' theme-night ' , cat : ' theme ' , name : ' Night Theme ' , cost : 150 , ico : ' 🌙 ' , equip : ' theme ' } ,       
] ;
let shopCat = ' mobilya ' ;   
$$ ( ' #shopTabs butonu ' ) . forEach ( b = > b . addEventListener ( ' click ' , ( ) = > {  
  $$ ( ' #shopTabs butonu ' ) . forEach ( x = > x . classList . remove ( ' active ' ) ) ;
b.classList.add ( ' active ' ) ; shopCat = b.dataset.cat ; renderShop ( ) ;​​​​​​​​      
} ) ) ;
fonksiyon renderShop ( ) { 
  $ ( ' #shopGrid ' ) . innerHTML = SHOP . filter ( i = > i . cat = = = shopCat ) . map ( i = > {  
sabit sahip olunan = S.inventory.includes ( i.id ) ;​​​​​       
return ` <div class="shop-item ${ owned ? ' owned ' : ' ' } ">     
      <div class="ico"> ${ i . ico } </div>
      <div class="name"> ${ i . name } </div>
      <div class="cost"> ${ i . cost } XP</div>
      <button data-buy=" ${ i . id } "> ${ owned ? ( i . equip ? ' Equip ' : ' Owned ' ) : ' Buy ' } </button>
    </div> ` ;
} ) . birleştir ( ' ' ) ;  
  $$ ( ' #shopGrid [data-buy] ' ) . forEach ( b = > b . addEventListener ( ' click ' , ( ) = > buy ( b . dataset . buy ) ) ) ;   
}
fonksiyon satın al ( id ) { 
const it = SHOP.find ( x = > x.id === id ) ; if ( ! it ) return ;​​​​​​       
eğer ( ! S . envanter . içerir ( id ) ) {  
eğer ( S.xp < it.cost ) { toast ( ' Yeterli XP yok ' ) ; return ; }​​         
S . xp - = it . cost ; S . inventory . push ( id ) ; save ( ) ;        
tost ( ' Satın Alındı ​​' + it . adı + ' 🎁 ' ) ; konfeti ( ) ;     
} aksi takdirde eğer ( it.equip ) {​    
S . donatıldı [ it . donat ] = id ; kaydet ( ) ; toast ( ' Donatıldı ' + it . ad ) ;        
}  
renderShop ( ) ; renderRoom ( ) ; renderHeader ( ) ;    
}
fonksiyon renderRoom ( ) { 
const sarma = $ ( '# roomItems ' ) ;     
// Bot hariç tüm öğeleri kaldır  
  $$ ( ' .item ' , wrap ) . forEach ( n = > n . remove ( ) ) ;  
MAĞAZA .filter ( i = > S.inventory.includes ( i.id ) && i.x && i.y ) .forEach ( i = > {​​​​​​​​​​​​​​       
const el = document.createElement ( ' div ' ) ;​​       
el . className = ' item ' ;    
el.style.left = i.x ; el.style.bottom = i.y ;​​​​​​​​​​​​         
el . style . fontSize = ' 2.6rem ' ;      
el . metinİçerik = ben . ico ;      
wrap . appendChild ( el ) ;    
} ) ;  
// tema  
  sabit t = S.donanımlı.tema ;​​​​   
sabit oda = $ ( ' #roomCanvas ' ) ;     
oda . stil . arka plan = t = = = ' tema-gecesi '    
? ' doğrusal-eğim(180 derece,#1B2440 0%,#2a356b 60%,#3a467d 100%) '     
: t = = = ' tema-pastel '     
? ' doğrusal-eğim(180 derece,#FFE5F1 0%,#E5EBFF 60%,#D8C2FF 100%) '       
: ' doğrusal-eğim(180 derece,#EAF1FF 0%,#DDE7FF 60%,#C8E3FF 100%) ' ;       
}
/* ---------- Analiz grafikleri (Canvas) ---------- */
fonksiyon çizim grafikleri ( ) { 
sabit günler = songünler ( 7 ) ;     
// KPI'lar  
  const toplam = gün . azalt ( ( a , d ) = > a + d . toplam , 0 ) | | 1 ;   
const corr = days . reduce ( ( a , d ) = > a + d . correct , 0 ) ;     
  $ ( ' #aWeekRate ' ) . textContent = Math . round ( corr / tot * 100 ) + ' % ' ;  
  $ ( ' #birHaftaDelta ' ) . textContent = ( düzeltme / toplam > .6 ? ' + ' : ' ' ) + Math .​ round ( ( corr / tot - .55 ) * 100 ) + ' % vs son ' ;  
const todayD = days [ days.length - 1 ] ; const todayTot = ( todayD.total || 1 ) ;​​​​​         
  $ ( ' #aDayRate ' ) . textContent = Math . round ( ( todayD . correct | | 0 ) / todayTot * 100 ) + ' % ' ;  
  $ ( ' #aUsage ' ) . textContent = days . reduce ( ( a , d ) = > a + ( d . minutes | | 0 ) , 0 ) + ' min ' ;  
  $ ( ' #aStreak ' ) . textContent = S . streak + ' 🔥 ' ;  
drawLine ( ' chartProgress ' , days.map ( d = > d.total ? d.correct / d.total * 100 : 0 ) , ' # 7DA8FF ' , ' % ' ) ;​​​​​​​         
drawBars ( ' chartXP ' , days.map ( d = > ( d.correct || 0 ) * 15 ) , ' # 9F8CFF ' ) ;​​​​     
drawDonut ( ' chartEmotion ' , emotionCounts ( days ) ) ;   
drawLine ( ' chartActivity ' , days.map ( d = > d.minutes || 0 ) , ' # A7E8D6 ' , ' m ' ) ;​​​​      
}
fonksiyon songünler ( n ) { 
sabit arr = [ ] ; sabit harita = { } ;     
S.history.forEach ( h = > map [ h.date ] = h ) ;​​​​​​   
for ( let i = n - 1 ; i > = 0 ; i -- ) {​   
const d = new Date ( ) ; d.setDate ( d.getDate ( ) - i ) ;​​​​       
sabit k = d . toISOString ( ) . slice ( 0 , 10 ) ;     
arr.push ( map [ k ] || ( k === today ( ) ? { tarih : k , correct : S._tcorrect || 0 , total : S._ttotal || 0 , minutes : S.todayMinutes || 0 , mood : S._tmood || ' nötr ' } : { tarih : k , correct : 0 , total : 0 , minutes : 0 , mood : ' nötr ' } ) ) ;​​​​​​​​​​​​​​​​​         
}  
arr'ı döndür ;   
}
fonksiyon emotionCounts ( günler ) { 
const out = { happy : 0 , excited : 0 , neutral : 0 , sad : 0 , tired : 0 } ;           
S . duygular . her biri için ( e = > { eğer ( out [ e . ruh hali ] ! = null ) out [ e . ruh hali ] + + ; } ) ;     
gün . forEach ( d = > { if ( out [ d . mood ] ! = null ) out [ d . mood ] + + ; } ) ;     
çıkış yap ;   
}
fonksiyon setupCanvas ( id ) { 
const c = document.getElementById ( id ) ; if ( ! c ) return null ;​​        
const dpr = window.devicePixelRatio || 1 ;​​​     
const rect = c . getBoundingClientRect ( ) ;     
c . genişlik = dikdörtgen . genişlik * dpr ; c . yükseklik = 260 * dpr ;       
const ctx = c.getContext ( ' 2d ' ) ; ctx.scale ( dpr , dpr ) ;​​​​      
return { ctx , w : rect . width , h : 260 } ;       
}
fonksiyon drawLine ( id , data , color , suffix = ' ' ) {    
const s = setupCanvas ( id ) ; if ( ! s ) return ; const { ctx , w , h } = s ;           
ctx . clearRect ( 0 , 0 , w , h ) ;  
// ızgara  
  ctx.strokeStyle = ' # EEF1FB ' ; ctx.lineWidth = 1 ;​​​ 
for ( let i = 0 ; i < 5 ; i + + ) { const y = 30 + i * ( h - 60 ) / 4 ; ctx . beginPath ( ) ; ctx . moveTo ( 40 , y ) ; ctx . lineTo ( w - 10 , y ) ; ctx . stroke ( ) ; }             
const max = Math.max ( ... data , suffix === ' % ' ? 100 : 10 ) ;​​​​​​      
sabit adım = ( w - 60 ) / ( veri.uzunluk - 1 ) ;​​     
// alan  
  const grad = ctx.createLinearGradient ( 0 , 0 , 0 , h ) ; grad.addColorStop ( 0 , color + ' 66 ' ) ; grad.addColorStop ( 1 , color + ' 00 ' ) ;​​​​​​     
ctx.beginPath ( ) ; data.forEach ( ( v , i ) = > { const x = 40 + i * step , y = 30 + ( 1 - v / max ) * ( h - 60 ) ; if ( i === 0 ) ctx.moveTo ( x , y ) ; else ctx.lineTo ( x , y ) ; } ) ;​​​​​​​​​​         
ctx.lineTo ( 40 + ( data.length - 1 ) * step , h - 30 ) ; ctx.lineTo ( 40 , h - 30 ) ; ctx.closePath ( ) ;​​​​​​​​    
ctx.fillStyle = grad ; ctx.fill ( ) ;​​​​   
// satır  
  ctx.beginPath ( ) ; data.forEach ( ( v , i ) = > { const x = 40 + i * step , y = 30 + ( 1 - v / max ) * ( h - 60 ) ; if ( i === 0 ) ctx.moveTo ( x , y ) ; else ctx.lineTo ( x , y ) ; } ) ;​​​​​​​​​​       
ctx.strokeStyle = color ; ctx.lineWidth = 3 ; ctx.lineJoin = ' round ' ; ctx.stroke ( ) ;​​​​​​​​     
// puanlar  
  data.forEach ( ( v , i ) = > { const x = 40 + i * step , y = 30 + ( 1 - v / max ) * ( h - 60 ) ; ctx.fillStyle = ' #fff ' ; ctx.beginPath ( ) ; ctx.arc ( x , y , 5 , 0 , Math.PI * 2 ) ; ctx.fill ( ) ; ctx.strokeStyle = color ; ctx.lineWidth = 2 ; ctx.stroke ( ) ; } ) ;​​​​​​​​​​​​​​​​​​           
// x etiketleri  
  ctx.fillStyle = ' # 8A93B2 ' ; ctx.font = ' 600 11px Plus Jakarta Sans ' ; ctx.textAlign = ' center ' ;​  
sabit etiketler = [ ' Pzt ' , ' Sal ' , ' Çar ' , ' Per ' , ' Cum ' , ' Cmt ' , ' Paz ' ] ;     
const start = ( new Date ( ) . getDay ( ) + 6 ) % 7 ; // Mon=0       
  for ( let i = 0 ; i < data.length ; i ++ ) { const x = 40 + i * step ; const lbl = labels [ ( start + i - data.length + 1 + 7 ) % 7 ] ; ctx.fillText ( lbl , x , h - 10 ) ; }​​​​​​​           
}
fonksiyon drawBars ( id , data , color ) {   
const s = setupCanvas ( id ) ; if ( ! s ) return ; const { ctx , w , h } = s ;         
ctx . clearRect ( 0 , 0 , w , h ) ;  
const max = Math.max ( ... data , 10 ) ; const bw = ( w - 60 ) / data.length - 10 ;​​​​​​            
veri . forEach ( ( v , i ) = > {  
    const x = 40 + i*((w-60)/data.length) + 5; const bh = (v/max)*(h-60); const y = h-30-bh;
    const grad = ctx.createLinearGradient(0,y,0,h-30); grad.addColorStop(0,color); grad.addColorStop(1,color+'55');
    ctx.fillStyle=grad; roundRect(ctx,x,y,bw,bh,6); ctx.fill();
    ctx.fillStyle='#1B2440'; ctx.font='700 11px Plus Jakarta Sans'; ctx.textAlign='center';
    ctx.fillText(v, x+bw/2, y-6);
  });
}
function roundRect ( ctx , x , y , w , h , r ) { ctx . beginPath ( ) ; ctx . moveTo ( x + r , y ) ; ctx . lineTo ( x + w - r , y ) ; ctx . quadraticCurveTo ( x + w , y , x + w , y + r ) ; ctx . lineTo ( x + w , y + h ) ; ctx . lineTo ( x , y + h ) ; ctx . lineTo ( x , y + r ) ; ctx . quadraticCurveTo ( x , y , x + r , y ) ; ctx . closePath ( ) ; }           
fonksiyon drawDonut ( id , counts ) {  
const s = setupCanvas ( id ) ; if ( ! s ) return ; const { ctx , w , h } = s ; ctx.clearRect ( 0 , 0 , w , h ) ;​​          
const total = Object.values ( counts ) .reduce ( ( a , b ) = > a + b , 0 ) || 1 ;​​​​     
sabit renkler = { mutlu : ' #FFD36E ' , heyecanlı : ' #FF9FB3 ' , nötr : ' #AFCBFF ' , üzgün : ' #9F8CFF ' , yorgun : ' #A7E8D6 ' } ;           
let start = - Math . PI / 2 ;     
sabit cx = w / 2 - 60 , cy = h / 2 , r = 90 ;             
Nesne . girişler ( sayılar ) . her biri için ( ( [ k , v ] ) = > {  
sabit açı = ( v / toplam ) * Math . PI * 2 ;       
ctx.beginPath ( ) ; ctx.moveTo ( cx , cy ) ; ctx.arc ( cx , cy , r , start , start + ang ) ; ctx.closePath ( ) ;​​​​​​​​       
ctx.fillStyle = colors [ k ] ; ctx.fill ( ) ;​​​​       
başlangıç ​​+ = açı ;      
} ) ;  
ctx.fillStyle = ' #fff ' ; ctx.beginPath ( ) ; ctx.arc ( cx , cy , 52 , 0 , Math.PI * 2 ) ; ctx.fill ( ) ;​​​​​​​​​​     
ctx . fillStyle = ' #1B2440 ' ; ctx . font = ' 800 18px Plus Jakarta Sans ' ; ctx . textAlign = ' center ' ;    
ctx.fillText ( total + ' ' , cx , cy - 2 ) ; ctx.font = ' 600 11px Plus Jakarta Sans ' ; ctx.fillStyle = ' # 8A93B2 ' ; ctx.fillText ( ' moods ' , cx , cy + 16 ) ;​​​         
// efsane  
  ly = 30 olsun ;   
Nesne . girişler ( sayılar ) . her biri için ( ( [ k , v ] ) = > {  
ctx.fillStyle = colors [ k ] ; ctx.fillRect ( w - 150 , ly , 14 , 14 ) ;​​​​       
ctx . fillStyle = ' #1B2440 ' ; ctx . font = ' 700 12px Plus Jakarta Sans ' ; ctx . textAlign = ' left ' ;      
ctx . fillText ( k + ' · ' + v , w - 130 , ly + 12 ) ; ly + = 22 ;       
} ) ;  
}
/* ---------- Premium ---------- */
fonksiyon renderPremium ( ) { 
  $ ( ' #premStatus ' ) . textContent = S . premium = = = ' free ' ? ' Ücretsiz Deneme ' : S . premium = = = ' starter ' ? ' Başlangıç ​​' : ' Terapist Destekli ' ;  
  $ ( ' #premTag ' ) . textContent = S . premium = = = ' free ' ? ' Ücretsiz Deneme ' : ' Aktif ' ;  
}
$$ ( ' #screen-premium [data-plan] ' ) . forEach ( b = > b . addEventListener ( ' click ' , e = > openPay ( e . currentTarget . dataset . plan ) ) ) ;   
fonksiyon openPay ( plan ) { 
const cfg = plan = = = ' başlangıç ​​' ? { name : ' Başlangıç ​​' , price : ' 250 ₺ ' , desc : ' Tüm aile için evde konuşma pratiği. ' }       
: { name : ' Terapist Destekli ' , price : ' 900 ₺ ' , desc : ' Konuşma terapisti ile işbirliğine dayalı bakım. ' } ;              
  $ ( ' #ödemePlan ' ) . metinİçerik = cfg . isim ; $ ( ' #ödeFiyat ' ) . metinİçerik = cfg . fiyat ; $ ( ' #payDesc ' ) . metinİçerik = cfg . tanım ;        
  $ ( ' #payConfirm ' ) . dataset . plan = plan ;  
  $ ( ' #payModal ' ) . classList . add ( ' active ' ) ;
}
$ ( ' #payConfirm ' ) . addEventListener ( ' click ' , e = > { 
const plan = e . currentTarget . dataset . plan ;     
S . premium = plan ; kaydet ( ) ;     
  $ ( ' #payModal ' ) . classList . remove ( ' active ' ) ;
tost ( ' 🎉 Abonelik etkinleştirildi — hoş geldiniz! ' ) ; konfeti ( ) ;   
renderPremium ( ) ; renderProfil ( ) ;   
} ) ;
/* ---------- Ayarlar ---------- */
$ ( ' #saveSettings ' ) . addEventListener ( ' click ' , ( ) = > { 
  S.settings.sfx = $('#setSfx').checked; S.settings.anim = $('#setAnim').checked;
  S.settings.remind = $('#setRemind').checked; S.settings.persona = $('#setPersona').value;
  save(); toast('Settings saved');
});
$('#switchMode').addEventListener('click', ()=> openParent());
$('#switchToChild').addEventListener('click', ()=> openChild());
$('#resetAll').addEventListener('click', ()=>{
  if(confirm('Reset all demo data?')){ localStorage.removeItem(KEY); S = clone(defaults); ensureToday(); renderAll(); toast('Demo reset'); }
});
/* ---------- Time tracking ---------- */
let activeTickTimer=null;
fonksiyon izleme zamanı ( ) { 
eğer ( aktifTickTimer ) ise geri dön ;   
activeTickTimer = setInterval ( ( ) = > { S . todayMinutes = ( S . todayMinutes | | 0 ) + 1 ; save ( ) ; renderHomeKPIs ( ) ; } , 60000 ) ;           
}
/* ---------- Tümünü Oluştur ---------- */
fonksiyon renderAll ( ) { 
ensureToday ( ) ; renderHeader ( ) ; renderHomeKPIs ( ) ; renderPath ( ) ; renderShop ( ) ; renderRoom ( ) ; renderProfile ( ) ; renderPremium ( ) ;         
// ayarlar  
  $ ( ' #setSfx ' ) . checked = ! ! S . settings . sfx ; $ ( ' #setAnim ' ) . checked = ! ! S . settings . anim ;     
  $ ( ' #setRemind ' ) . checked = ! ! S . settings . remind ; $ ( ' #setPersona ' ) . value = S . settings . persona ;     
}
/* ============================================================
   OYUNLAR
   ============================================================== */
const gameCanvas = document.getElementById ( ' gameCanvas ' ) ;​​   
const gctx = gameCanvas.getContext ( ' 2d ' ) ;​​   
let game = { type : null , raf : null , state : null } ;       
fonksiyon openGame ( tip ) { 
oyun.tür = tür ; $ ( ' #gameTitle ' ) . metinİçeriği = ( { tren : ' Hece Treni ' , merdivenler : ' Hece Merdivenleri ' , balık : ' Balık Avı ' , balon : ' Balon ' , tekerlek : ' Harf Çarkı ' } ) [ tür ] ;       
  $ ( ' #gamePrompt ' ) . textContent = ' Başlat ' düğmesine basın ;  
  $ ( ' #gameChoices ' ) . innerHTML = ' ' ;
  $ ( ' #oyunPuanı ' ) . metinİçeriği = ' 0 ' ; $ ( ' #oyunHayatları ' ) . metinİçeriği = ' 3 ' ; 
  $ ( ' #gameModal ' ) . classList . add ( ' active ' ) ;
initGame ( type ) ;  
}
$$ ( ' [data-game] ' ) . forEach ( b = > b . addEventListener ( ' click ' , ( ) = > openGame ( b . dataset . game ) ) ) ;   
$ ( ' #gameRestart ' ) . addEventListener ( ' click ' , ( ) = > initGame ( game . type ) ) ;  
$ ( ' #gameStart ' ) . addEventListener ( ' click ' , ( ) = > startGame ( ) ) ;  
fonksiyon stopGameLoop ( ) { eğer ( game . raf ) { cancelAnimationFrame ( game . raf ) ; game . raf = null ; } }      
fonksiyon initGame ( tip ) { 
stopGameLoop ( ) ;  
  $ ( ' #oyunPuanı ' ) . metinİçeriği = ' 0 ' ; $ ( ' #oyunHayatları ' ) . metinİçeriği = ' 3 ' ; 
oyun.durum = { puan : 0 , can : 3 , koşuyor : false } ;​​        
eğer ( tip = == ' tren ' ) initTrain ( ) ;​   
aksi takdirde eğer ( tip = == ' merdiven ' ) initStairs ( ) ;    
aksi takdirde eğer ( tip = = = ' balık ' ) initFish ( ) ;    
aksi halde eğer ( tip = = = ' balon ' ) initBalloon ( ) ;    
aksi takdirde eğer ( tip = = = ' tekerlek ' ) tekerleği başlat ( ) ;    
drawGame ( ) ;  
}
fonksiyon startGame ( ) { 
oyun.durum.çalışıyor = doğru ;​​​​    
döngü ( ) ;  
}
fonksiyon döngüsü ( ) { 
eğer ( ! oyun . durumu . çalışıyor ) geri dön ;   
updateGame ( ) ; drawGame ( ) ;   
oyun . raf = requestAnimationFrame ( döngü ) ;    
}
fonksiyon oyunKazanma ( ödül = 40 ) { 
oyun.durum.çalışıyor = false ; OyunDöngüsünü durdur ( ) ;​​​   
tost ( ' 🏆 Kazandınız! + ' + ödül + ' XP ' ) ; XP ekle ( ödül , ' oyun ' ) ; konfeti ( ) ;    
eğer ( ! S . envanter . ' gamewin ' ) ) S . envanter . ' gamewin ' ) ;​​​​   
  $ ( ' #gamePrompt ' ) . textContent = ' Kazandınız! 🎉 ' ;  
}
oyunbitti fonksiyonu ( ) { 
oyun.durum.çalışıyor = false ; OyunDöngüsünü durdur ( ) ;​​​   
toast ( ' Tekrar dene — başaracaksın! ' ) ; $ ( ' #gamePrompt ' ) . textContent = ' Oyun bitti — yeniden başlat? ' ;   
}
fonksiyon setChoices ( arr , cb ) {  
const el = $ ( ' #gameChoices ' ) ; el . innerHTML = ' ' ;      
arr.forEach ( v = > { const b = document.createElement ( ' button ' ) ; b.textContent = v ; b.onclick = ( ) = > cb ( v ) ; el.appendChild ( b ) ; } ) ;​​​​​​​​​​        
}
fonksiyon randItem ( a ) { return a [ Math.floor ( Math.random ( ) * a.length ) ] ; }​​​​​​    
/* ---- Çöp Adam ---- */
fonksiyon drawStickman ( x , y , scale = 1 , armSwing = 0 ) { 
gctx.save ( ) ; gctx.translate ( x , y ) ; gctx.scale ( scale , scale ) ;​​​​​​    
gctx . strokeStyle = ' #1B2440 ' ; gctx . lineWidth = 3 ; gctx . lineCap = ' round ' ;    
gctx . fillStyle = ' #FFD36E ' ;  
gctx.beginPath ( ) ; gctx.arc ( 0 , -30 , 10 , 0 , Math.PI * 2 ) ; gctx.fill ( ) ; gctx.stroke ( ) ;​​​​​​​​​​​     
gctx . beginPath ( ) ; gctx . moveTo ( 0 , - 20 ) ; gctx . lineTo ( 0 , 10 ) ; gctx . stroke ( ) ;     
gctx.beginPath ( ) ; gctx.moveTo ( 0 , -12 ) ; gctx.lineTo ( -12 , -2 + armSwing ) ; gctx.moveTo ( 0 , -12 ) ; gctx.lineTo ( 12 , -2 - armSwing ) ; gctx.stroke ( ) ;​​​​​​​​​​​​​​​​​       
gctx . beginPath ( ) ; gctx . moveTo ( 0 , 10 ) ; gctx . lineTo ( - 8 , 24 ) ; gctx . moveTo ( 0 , 10 ) ; gctx . lineTo ( 8 , 24 ) ; gctx . stroke ( ) ;       
gctx . restore ( ) ;  
}
/* ---- TREN ---- */
fonksiyon initTrain ( ) { 
const cars = [ ] ; const W = gameCanvas . width / ( window . devicePixelRatio | | 1 ) ;         
for ( let i = 0 ; i < 5 ; i + + ) cars . push ( { x : 600 + i * 150 , syl : randItem ( BANK . syllables ) } ) ;           
oyun.durum.arabalar = arabalar ; oyun.durum.oyuncu = { araba : 0 , zıplama : 0 , y : 0 } ;​​​​​​​​           
oyun.durum.hedef = arabalar [ 1 ] .syl ; oyun.durum.kaydırma = 0 ;​​​​​​​​​       
  $ ( ' #gamePrompt ' ) . textContent = ' Hedef: ' + game . state . target ;  
setChoices ( shuffle ( [ game . state . target , randItem ( BANK . syllables ) , randItem ( BANK . syllables ) ] ) , v = > {     
eğer ( v === oyun.durum.hedef ) {​​​​​​    
oyun.durum.skor + = 10 ; oyun.durum.oyuncu.araba = Math.min ( arabalar.uzunluk - 1 , oyun.durum.oyuncu.araba + 1 ) ; oyun.durum.oyuncu.zıplama = 16 ;​​​​​​​​​​​​​​​​​​​​​​​​​​             
addXP ( 10 , ' train ' ) ;      
eğer ( oyun.durum.oyuncu.araba > = arabalar.uzunluk - 1 ) { oyunkazandı ( 50 ) ; geri dön ; }​​​​​​​         
oyun.durum.hedef = arabalar [ oyun.durum.oyuncu.araba + 1 ] .syl ;​​​​​​​​​​​        
      $ ( ' #gamePrompt ' ) . textContent = ' Hedef: ' + game . state . target ;
setChoices ( shuffle ( [ game . state . target , randItem ( BANK . syllables ) , randItem ( BANK . syllables ) ] ) , arguments . callee ) ;         
} else { game.state.lives -- ; $ ( ' # gameLives ' ) . textContent = game.state.lives ; if ( game.state.lives < = 0 ) gameOver ( ) ; }​​​​​​​​​​​​           
    $ ( ' #gameScore ' ) . textContent = game . state . score ;
} ) ;  
}
fonksiyon updateTrain ( ) { 
oyun . durum . kaydırma + = 1 . 4 ;    
eğer ( oyun.durum.oyuncu.zıplama > 0 ) oyun.durum.oyuncu.zıplama - = 1 ;​​​​​​​​​​​​   
}
fonksiyon drawTrain ( ) { 
const W = gameCanvas . width / ( devicePixelRatio | | 1 ) , H = gameCanvas . height / ( devicePixelRatio | | 1 ) ;    
const sky = gctx.createLinearGradient ( 0 , 0 , 0 , H ) ; sky.addColorStop ( 0 , ' # EAF1FF ' ) ; sky.addColorStop ( 1 , ' # C8E3FF ' ) ;​​​​       
gctx . fillStyle = sky ; gctx . fillRect ( 0 , 0 , W , H ) ;   
// zemin  
  gctx.fillStyle = ' # A7E8D6 ' ; gctx.fillRect ( 0 , H - 60 , W , 60 ) ;​​​ 
// raylar  
  gctx.fillStyle = ' # 7A8AB0 ' ; gctx.fillRect ( 0 , H - 90 , W , 6 ) ;​​​ 
// arabalar  
  oyun . durum . arabalar . her biri için ( ( c , i ) = > {
sabit x = 80 + i * 150 - ( oyun . durum . kaydırma % 150 ) ;           
gctx . fillStyle = i = = = 0 ? ' #7DA8FF ' : ' #9F8CFF ' ;      
roundRect ( gctx , x , H - 160 , 120 , 70 , 12 ) ; gctx . fill ( ) ;     
gctx . fillStyle = ' #fff ' ; gctx . yazı tipi = ' 800 22px Plus Jakarta Sans ' ; gctx . textAlign = ' merkez ' ;      
gctx . fillText ( c.syl , x + 60 , H - 115 ) ;​​      
gctx.fillStyle = ' # 1B2440 ' ; gctx.beginPath ( ) ; gctx.arc ( x + 25 , H - 80 , 10 , 0 , Math.PI * 2 ) ; gctx.arc ( x + 95 , H - 80 , 10 , 0 , Math.PI * 2 ) ; gctx.fill ( ) ;​​​​​​​​​​​​​        
} ) ;  
// mevcut araçtaki oyuncu  
  const pc = game.state.player.car ;​​​​​​   
const px = 80 + pc * 150 - ( game . state . scroll % 150 ) + 60 ;           
drawStickman ( px , H - 160 - 6 - game . state . player . jumping * 2 , 1 , Math . sin ( performance . now ( ) / 120 ) * 3 ) ;         
}
/* ---- MERDİVENLER ---- */
function initStairs ( ) { 
sabit adımlar = 8 ; oyun.durum.adımlar = adımlar ;​​​​        
oyun.durum.konum = 0 ; oyun.durum.syl = rastgele öğe ( BANK.sylables ) ;​​​​​​​​​       
  $ ( ' #gamePrompt ' ) . textContent = ' Tekrarla: ' + game . state . syl ;
sabit işleyici = v = > {     
eğer ( v === game.state.syl ) { game.state.pos ++ ; game.state.score + = 10 ; addXP ( 8 , ' merdiven ' ) ; eğer ( game.state.pos > = steps ) { gameWin ( 60 ) ; return ; } }​​​​​​​​​​​​​​​​​​​            
aksi halde { oyun.durum.konum = Math.max ( 0 , oyun.durum.konum - 1 ) ; oyun.durum.canlar -- ; $ ( ' # gameLives ' ) . textContent = oyun.durum.canlar ; eğer ( oyun.durum.canlar < = 0 ) { oyun bitti ( ) ; geri dön ; } }​​​​​​​​​​​​​​​​​​​          
oyun . durum . hece = rastgele öğe ( BANK . heceler ) ;      
    $ ( ' #gamePrompt ' ) . textContent = ' Tekrarla: ' + game . state . syl ;
    $ ( ' #gameScore ' ) . textContent = game . state . score ;
setChoices ( shuffle ( [ game . state . syl , randItem ( BANK . syllables ) , randItem ( BANK . syllables ) ] ) , handler ) ;       
} ;  
setChoices ( shuffle ( [ game . state . syl , randItem ( BANK . syllables ) , randItem ( BANK . syllables ) ] ) , handler ) ;     
}
fonksiyon updateStairs ( ) { } 
fonksiyon drawStairs ( ) { 
const W = gameCanvas . width / ( devicePixelRatio | | 1 ) , H = gameCanvas . height / ( devicePixelRatio | | 1 ) ;    
const sky = gctx.createLinearGradient ( 0 , 0 , 0 , H ) ; sky.addColorStop ( 0 , ' # D8C2FF ' ) ; sky.addColorStop ( 1 , ' #AFCBFF ' ) ;​​​​​       
gctx . fillStyle = sky ; gctx . fillRect ( 0 , 0 , W , H ) ;   
sabit adımlar = oyun.durum.adımlar ; sabit sw = 70 , sh = 28 ;​​​​        
for ( let i = 0 ; i < steps ; i + + ) {   
sabit x = 100 + i * sw ; sabit y = H - 60 - i * sh ;                 
gctx . fillStyle = i < game . state . pos ? ' #A7E8D6 ' : ' #fff ' ;      
roundRect ( gctx , x , y , sw + 4 , sh , 8 ) ; gctx . fill ( ) ;     
gctx . fillStyle = ' #1B2440 ' ; gctx . font = ' 700 13px Plus Jakarta Sans ' ; gctx . textAlign = ' center ' ;      
gctx . fillText ( i + 1 , x + sw / 2 , y + 18 ) ;      
}  
  const px = 100 + game.state.pos*sw + sw/2;
  const py = H - 60 - game.state.pos*sh - 8;
  drawStickman(px, py, 0.9, Math.sin(performance.now()/200)*3);
  if(game.state.pos>=steps){ gctx.fillStyle='#FFD36E'; gctx.font='800 28px Plus Jakarta Sans'; gctx.fillText('🏆', px, py-30); }
}
/* ---- FISH ---- */
function initFish(){
  game.state.fishes = []; game.state.caught = 0; game.state.target = randItem(BANK.letters);
  for(let i=0;i<6;i++) game.state.fishes.push(makeFish());
  $('#gamePrompt').textContent='Catch: '+game.state.target;
  setChoices([], ()=>{});
}
function makeFish(){
  const W=gameCanvas.width/(devicePixelRatio||1), H=gameCanvas.height/(devicePixelRatio||1);
  const isTarget = Math.random()<.5;
  return { x: Math.random()*W, y: H/2 + 40 + Math.random()*100, vx:(Math.random()*1.6+.6)*(Math.random()<.5?-1:1), c: isTarget?'#FFD36E':randItem(['#9F8CFF','#7DA8FF','#A7E8D6','#FF9FB3']), letter: isTarget?game.state.target:randItem(BANK.letters), isTarget };
}
function updateFish(){
  const W=gameCanvas.width/(devicePixelRatio||1);
  game.state.fishes.forEach(f=>{ f.x+=f.vx; if(f.x<-30||f.x>W+30) f.vx*=-1; });
}
function drawFish(){
  const W=gameCanvas.width/(devicePixelRatio||1), H=gameCanvas.height/(devicePixelRatio||1);
  // sky+water
  const sky = gctx.createLinearGradient(0,0,0,H/2); sky.addColorStop(0,'#C8E3FF'); sky.addColorStop(1,'#EAF1FF');
  gctx.fillStyle=sky; gctx.fillRect(0,0,W,H/2);
  const water = gctx.createLinearGradient(0,H/2,0,H); water.addColorStop(0,'#7DA8FF'); water.addColorStop(1,'#3a5fd1');
  gctx.fillStyle=water; gctx.fillRect(0,H/2,W,H/2);
  // waves
  gctx.strokeStyle='#ffffff66'; gctx.lineWidth=2;
  for(let i=0;i<4;i++){ gctx.beginPath(); const y=H/2+i*30+Math.sin(performance.now()/400+i)*3;
    for(let x=0;x<=W;x+=20) gctx.lineTo(x, y+Math.sin((x+performance.now()/8)/30)*3); gctx.stroke(); }
  // boat
  gctx.fillStyle='#8B5A2B'; const bx=W/2-50, by=H/2-10;
  gctx.beginPath(); gctx.moveTo(bx,by); gctx.lineTo(bx+100,by); gctx.lineTo(bx+85,by+30); gctx.lineTo(bx+15,by+30); gctx.closePath(); gctx.fill();
  drawStickman(W/2, by-6, 0.95, Math.sin(performance.now()/300)*4);
  // fish
  game.state.fishes.forEach(f=>{
    gctx.save(); gctx.translate(f.x,f.y); if(f.vx<0) gctx.scale(-1,1);
    gctx.fillStyle=f.c; gctx.beginPath(); gctx.ellipse(0,0,22,12,0,0,Math.PI*2); gctx.fill();
    gctx.beginPath(); gctx.moveTo(-20,0); gctx.lineTo(-32,-10); gctx.lineTo(-32,10); gctx.closePath(); gctx.fill();
    gctx.fillStyle='#1B2440'; gctx.beginPath(); gctx.arc(10,-3,2,0,Math.PI*2); gctx.fill();
    gctx.fillStyle='#fff'; gctx.font='800 11px Plus Jakarta Sans'; gctx.textAlign='center'; gctx.fillText(f.letter,0,4);
    gctx.restore();
  });
  // HUD
  gctx.fillStyle='#1B2440'; gctx.font='800 16px Plus Jakarta Sans'; gctx.textAlign='left';
  gctx.fillText('Caught: '+game.state.caught+'/5', 16, 26);
}
gameCanvas.addEventListener('click', e=>{
  if(game.type!=='fish' || !game.state || !game.state.running) return;
  const r=gameCanvas.getBoundingClientRect();
  const x=(e.clientX-r.left)*(gameCanvas.width/r.width)/(devicePixelRatio||1);
  const y=(e.clientY-r.top)*(gameCanvas.height/r.height)/(devicePixelRatio||1);
  const idx = game.state.fishes.findIndex(f=> Math.abs(f.x-x)<26 && Math.abs(f.y-y)<16);
  if(idx>-1){
    const f = game.state.fishes[idx];
    if(f.isTarget){ game.state.caught++; game.state.score+=10; addXP(10,'fish'); $('#gameScore').textContent=game.state.score; if(game.state.caught>=5){ gameWin(70); return; } }
    else { game.state.lives--; $('#gameLives').textContent=game.state.lives; if(game.state.lives<=0){gameOver(); return;} }
    game.state.fishes[idx] = makeFish();
  }
});
/* ---- BALLOON ---- */
function initBalloon(){
  game.state.size = 30; game.state.target = 160; game.state.word = randItem(BANK.nouns);
  $('#gamePrompt').textContent='Say & type: '+game.state.word;
  const handler = ()=>{
    const val = ($('#balloonIn')?.value||'').trim().toLowerCase();
    if(val===game.state.word){ game.state.size += 22; game.state.score+=10; addXP(10,'balloon'); if(game.state.size>=game.state.target){ gameWin(60); return; } game.state.word=randItem(BANK.nouns); $('#gamePrompt').textContent='Say & type: '+game.state.word; }
    else { game.state.lives--; $('#gameLives').textContent=game.state.lives; if(game.state.lives<=0){gameOver(); return;} }
    $('#gameScore').textContent=game.state.score;
    if($('#balloonIn')) $('#balloonIn').value='';
  };
  $('#gameChoices').innerHTML = `<input id="balloonIn" class="ex-input" style="width:200px" placeholder="Type the word"/> <button class="btn btn-primary" id="balloonGo">Blow!</button>`;
  $('#balloonGo').onclick = handler;
  $('#balloonIn').addEventListener('keydown', e=>{ if(e.key==='Enter') handler(); });
}
function updateBalloon(){}
function drawBalloon(){
  const W=gameCanvas.width/(devicePixelRatio||1), H=gameCanvas.height/(devicePixelRatio||1);
  const sky = gctx.createLinearGradient(0,0,0,H); sky.addColorStop(0,'#BEEFE6'); sky.addColorStop(1,'#C8E3FF');
  gctx.fillStyle=sky; gctx.fillRect(0,0,W,H);
  // clouds
  gctx.fillStyle='#ffffffcc';
  [[80,60],[300,40],[700,80]].forEach(([x,y])=>{ gctx.beginPath(); gctx.arc(x,y,18,0,Math.PI*2); gctx.arc(x+18,y+4,22,0,Math.PI*2); gctx.arc(x+38,y,18,0,Math.PI*2); gctx.fill(); });
  const cx=W/2, cy=H/2; const r = game.state.size;
  // string
  gctx.strokeStyle='#1B2440'; gctx.lineWidth=2; gctx.beginPath(); gctx.moveTo(cx, cy+r); gctx.quadraticCurveTo(cx-12, cy+r+40, cx, cy+r+80); gctx.stroke();
  // balloon
  const grad = gctx.createRadialGradient(cx-r/3, cy-r/3, r/5, cx, cy, r);
  grad.addColorStop(0,'#FFE2EC'); grad.addColorStop(1,'#FF7E9A');
  gctx.fillStyle=grad; gctx.beginPath(); gctx.ellipse(cx,cy,r,r*1.15,0,0,Math.PI*2); gctx.fill();
  gctx.fillStyle='#FF7E9A'; gctx.beginPath(); gctx.moveTo(cx-6,cy+r*1.15); gctx.lineTo(cx+6,cy+r*1.15); gctx.lineTo(cx,cy+r*1.15+10); gctx.closePath(); gctx.fill();
  // size %
  gctx.fillStyle='#1B2440'; gctx.font='800 14px Plus Jakarta Sans'; gctx.textAlign='center';
  gctx.fillText(Math.round(r/game.state.target*100)+'%', cx, cy+4);
}
/* ---- WHEEL ---- */
function initWheel(){
  game.state.angle=0; game.state.spinning=false; game.state.letters = ['A','E','I','O','U','S','T','R','N','L','P','M'];
  game.state.collected=[]; game.state.won=0;
  $('#gamePrompt').textContent='Spin the wheel & build words!';
  $('#gameChoices').innerHTML = `<button class="btn btn-primary" id="spinBtn">Spin 🎡</button>
    <input id="wordIn" class="ex-input" style="width:180px" placeholder="Type a word"/>
    <button class="btn btn-warm" id="checkWord">Check</button>`;
  $('#spinBtn').onclick = ()=>{ if(game.state.spinning) return; game.state.spinning=true; game.state.spinTarget = game.state.angle + 5*Math.PI + Math.random()*Math.PI*2; };
  $('#checkWord').onclick = ()=>{
    const w = ($('#wordIn').value||'').trim().toUpperCase();
    if(w.length<3){ toast('At least 3 letters'); return; }
    // valid if all letters appear in current wheel + collected pool
    const pool = (game.state.letters.concat(game.state.collected)).join('');
    let ok = true; const pc = pool.split('');
    for(const c of w){ const idx=pc.indexOf(c); if(idx<0){ ok=false; break; } pc.splice(idx,1); }
    if(ok){ game.state.won++; game.state.score+=20; addXP(20,'wheel'); $('#gameScore').textContent=game.state.score; toast('Nice word! +20 XP'); $('#wordIn').value=''; if(game.state.won>=3){ gameWin(80); } }
    else { toast('Letters not in the wheel — spin again'); game.state.lives--; $('#gameLives').textContent=game.state.lives; if(game.state.lives<=0) gameOver(); }
  };
}
function updateWheel(){
  if(game.state.spinning){
    const diff = game.state.spinTarget - game.state.angle;
    game.state.angle += diff*0.04;
    if(Math.abs(diff)<0.01){ game.state.spinning=false; game.state.angle = game.state.spinTarget;
      // collect pointer letter
      const n=game.state.letters.length;
      const idx = Math.floor(((-game.state.angle/(Math.PI*2))*n)%n + n)%n;
      const l = game.state.letters[idx]; game.state.collected.push(l);
      $('#gamePrompt').textContent='Got: '+l+' · Pool: '+game.state.collected.join(' ');
    }
  }
}
function drawWheel(){
  const W=gameCanvas.width/(devicePixelRatio||1), H=gameCanvas.height/(devicePixelRatio||1);
  const bg = gctx.createLinearGradient(0,0,0,H); bg.addColorStop(0,'#FFE5F1'); bg.addColorStop(1,'#D8C2FF');
  gctx.fillStyle=bg; gctx.fillRect(0,0,W,H);
  const cx=W/2, cy=H/2+10, r=130;
  const letters = game.state.letters; const n=letters.length;
  for(let i=0;i<n;i++){
    const a0 = game.state.angle + i*(Math.PI*2/n), a1 = a0+Math.PI*2/n;
    gctx.beginPath(); gctx.moveTo(cx,cy); gctx.arc(cx,cy,r,a0,a1); gctx.closePath();
    gctx.fillStyle = i%2? '#9F8CFF':'#7DA8FF'; gctx.fill();
    gctx.save(); gctx.translate(cx,cy); gctx.rotate(a0+(Math.PI*2/n)/2);
    gctx.fillStyle='#fff'; gctx.font='800 18px Plus Jakarta Sans'; gctx.textAlign='right';
    gctx.fillText(letters[i], r-14, 6); gctx.restore();
  }
  // hub
  gctx.fillStyle='#fff'; gctx.beginPath(); gctx.arc(cx,cy,28,0,Math.PI*2); gctx.fill();
  gctx.fillStyle='#1B2440'; gctx.font='800 14px Plus Jakarta Sans'; gctx.textAlign='center'; gctx.fillText('SPIN', cx, cy+5);
  // pointer
  gctx.fillStyle='#FFD36E'; gctx.beginPath(); gctx.moveTo(cx, cy-r-2); gctx.lineTo(cx-12,cy-r-22); gctx.lineTo(cx+12,cy-r-22); gctx.closePath(); gctx.fill();
}
/* ---- generic dispatch ---- */
function updateGame(){ if(game.type==='train') updateTrain(); else if(game.type==='stairs') updateStairs(); else if(game.type==='fish') updateFish(); else if(game.type==='balloon') updateBalloon(); else if(game.type==='wheel') updateWheel(); }
function drawGame(){
  const W=gameCanvas.width, H=gameCanvas.height; gctx.setTransform(1,0,0,1,0,0); gctx.clearRect(0,0,W,H);
  const dpr = window.devicePixelRatio||1; gameCanvas.width = gameCanvas.clientWidth*dpr; gameCanvas.height = 380*dpr; gctx.scale(dpr,dpr);
  if(game.type==='train') drawTrain();
  else if(game.type==='stairs') drawStairs();
  else if(game.type==='fish') drawFish();
  else if(game.type==='balloon') drawBalloon();
  else if(game.type==='wheel') drawWheel();
}
function shuffle(a){ a=a.slice(); for(let i=a.length-1;i>0;i--){ const j=Math.floor(Math.random()*(i+1)); [a[i],a[j]]=[a[j],a[i]]; } return a; }
/* ---- close gameModal stops loop ---- */
$$('#gameModal [data-close]').forEach(b=> b.addEventListener('click', ()=>{ stopGameLoop(); game.state && (game.state.running=false); }));
/* ============================================================
   PARENT DASHBOARD
   ============================================================ */
function renderParent(){
  const days = lastNDays(7);
  const tot = days.reduce((a,d)=>a+d.total,0)||1;
  const corr = days.reduce((a,d)=>a+d.correct,0);
  $('#pa_rate').textContent = Math.round(corr/tot*100)+'%';
  $('#pa_sessions').textContent = days.filter(d=> d.total>0).length;
  $('#pa_time').textContent = days.reduce((a,d)=>a+(d.minutes||0),0)+'m';
  drawParentCharts(days);
  renderNotes(); renderSpeechSummary();
}
function drawParentCharts(days){
  drawLine('pa_chartProgress', days.map(d=> d.total? d.correct/d.total*100:0), '#7DA8FF','%');
  drawDonut('pa_chartEmo', emotionCounts(days));
}
$('#addNote').addEventListener('click', ()=>{
  const v = $('#therapistNote').value.trim(); if(!v) return;
  S.notes.unshift({date: new Date().toLocaleString(), text:v}); save(); $('#therapistNote').value=''; renderNotes();
});
function renderNotes(){
  $('#notesList').innerHTML = S.notes.map(n=> `<div class="note-box"><small>${n.date}</small><div style="margin-top:4px">${escapeHTML(n.text)}</div></div>`).join('') || `<p style="color:var(--c-muted)">No notes yet.</p>`;
}
function renderSpeechSummary(){
  $('#speechSummary').innerHTML = STAGES.map(st=>{
    const p = S.stages[st.id]||0; const pct = Math.round((p/5)*100);
    return `<div class="note-box" style="text-align:center"><b>${st.label}</b><div style="margin-top:6px;font-size:1.4rem">${pct}%</div><div class="bar" style="height:6px;background:#EEF1FB;border-radius:999px;overflow:hidden;margin-top:6px"><i style="display:block;height:100%;width:${pct}%;background:var(--grad-dark)"></i></div></div>`;
  }).join('');
}
function escapeHTML(s){ return s.replace(/[&<>'"]/g, c=>({'&':'&amp;','<':'&lt;','>':'&gt;',"'":'&#39;','"':'&quot;'}[c])); }
/* ---------- Init ---------- */
ensureToday(); renderAll();
// expose for debugging
window.NeuroBloom = { S, addXP, openGame, openExercise };
})();
