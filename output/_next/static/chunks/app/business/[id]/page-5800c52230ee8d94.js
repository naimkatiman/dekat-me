(self.webpackChunk_N_E=self.webpackChunk_N_E||[]).push([[357],{1567:(e,a,t)=>{"use strict";t.d(a,{cn:()=>i});var n=t(3463),r=t(9795);function i(){for(var e=arguments.length,a=Array(e),t=0;t<e;t++)a[t]=arguments[t];return(0,r.QP)((0,n.$)(a))}},6724:(e,a,t)=>{"use strict";t.d(a,{Tabs:()=>G,TabsContent:()=>E,TabsList:()=>I,TabsTrigger:()=>_});var n=t(5155),r=t(2115),i=t(3610),s=t(8166),o=t(7357),l=t(7028),d=t(3360),c=t(4256),u=t(1488),f=t(7668),b="Tabs",[v,m]=(0,s.A)(b,[o.RG]),g=(0,o.RG)(),[p,h]=v(b),y=r.forwardRef((e,a)=>{let{__scopeTabs:t,value:r,onValueChange:i,defaultValue:s,orientation:o="horizontal",dir:l,activationMode:b="automatic",...v}=e,m=(0,c.jH)(l),[g,h]=(0,u.i)({prop:r,onChange:i,defaultProp:s});return(0,n.jsx)(p,{scope:t,baseId:(0,f.B)(),value:g,onValueChange:h,orientation:o,dir:m,activationMode:b,children:(0,n.jsx)(d.sG.div,{dir:m,"data-orientation":o,...v,ref:a})})});y.displayName=b;var x="TabsList",w=r.forwardRef((e,a)=>{let{__scopeTabs:t,loop:r=!0,...i}=e,s=h(x,t),l=g(t);return(0,n.jsx)(o.bL,{asChild:!0,...l,orientation:s.orientation,dir:s.dir,loop:r,children:(0,n.jsx)(d.sG.div,{role:"tablist","aria-orientation":s.orientation,...i,ref:a})})});w.displayName=x;var N="TabsTrigger",j=r.forwardRef((e,a)=>{let{__scopeTabs:t,value:r,disabled:s=!1,...l}=e,c=h(N,t),u=g(t),f=T(c.baseId,r),b=k(c.baseId,r),v=r===c.value;return(0,n.jsx)(o.q7,{asChild:!0,...u,focusable:!s,active:v,children:(0,n.jsx)(d.sG.button,{type:"button",role:"tab","aria-selected":v,"aria-controls":b,"data-state":v?"active":"inactive","data-disabled":s?"":void 0,disabled:s,id:f,...l,ref:a,onMouseDown:(0,i.m)(e.onMouseDown,e=>{s||0!==e.button||!1!==e.ctrlKey?e.preventDefault():c.onValueChange(r)}),onKeyDown:(0,i.m)(e.onKeyDown,e=>{[" ","Enter"].includes(e.key)&&c.onValueChange(r)}),onFocus:(0,i.m)(e.onFocus,()=>{let e="manual"!==c.activationMode;v||s||!e||c.onValueChange(r)})})})});j.displayName=N;var C="TabsContent",R=r.forwardRef((e,a)=>{let{__scopeTabs:t,value:i,forceMount:s,children:o,...c}=e,u=h(C,t),f=T(u.baseId,i),b=k(u.baseId,i),v=i===u.value,m=r.useRef(v);return r.useEffect(()=>{let e=requestAnimationFrame(()=>m.current=!1);return()=>cancelAnimationFrame(e)},[]),(0,n.jsx)(l.C,{present:s||v,children:t=>{let{present:r}=t;return(0,n.jsx)(d.sG.div,{"data-state":v?"active":"inactive","data-orientation":u.orientation,role:"tabpanel","aria-labelledby":f,hidden:!r,id:b,tabIndex:0,...c,ref:a,style:{...e.style,animationDuration:m.current?"0s":void 0},children:r&&o})}})});function T(e,a){return"".concat(e,"-trigger-").concat(a)}function k(e,a){return"".concat(e,"-content-").concat(a)}R.displayName=C;var D=t(1567);let G=y,I=r.forwardRef((e,a)=>{let{className:t,...r}=e;return(0,n.jsx)(w,{ref:a,className:(0,D.cn)("inline-flex h-9 items-center justify-center rounded-lg bg-muted p-1 text-muted-foreground",t),...r})});I.displayName=w.displayName;let _=r.forwardRef((e,a)=>{let{className:t,...r}=e;return(0,n.jsx)(j,{ref:a,className:(0,D.cn)("inline-flex items-center justify-center whitespace-nowrap rounded-md px-3 py-1 text-sm font-medium ring-offset-background transition-all focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 data-[state=active]:bg-background data-[state=active]:text-foreground data-[state=active]:shadow",t),...r})});_.displayName=j.displayName;let E=r.forwardRef((e,a)=>{let{className:t,...r}=e;return(0,n.jsx)(R,{ref:a,className:(0,D.cn)("mt-2 ring-offset-background focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2",t),...r})});E.displayName=R.displayName},8336:(e,a,t)=>{Promise.resolve().then(t.t.bind(t,8173,23)),Promise.resolve().then(t.bind(t,6724))}},e=>{var a=a=>e(e.s=a);e.O(0,[345,934,441,587,358],()=>a(8336)),_N_E=e.O()}]);