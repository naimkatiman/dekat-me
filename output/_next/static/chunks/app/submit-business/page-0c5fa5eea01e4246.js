(self.webpackChunk_N_E=self.webpackChunk_N_E||[]).push([[304],{1027:(e,s,r)=>{"use strict";r.d(s,{F:()=>a});var n=r(3463);let t=e=>"boolean"==typeof e?`${e}`:0===e?"0":e,i=n.$,a=(e,s)=>r=>{var n;if((null==s?void 0:s.variants)==null)return i(e,null==r?void 0:r.class,null==r?void 0:r.className);let{variants:a,defaultVariants:d}=s,o=Object.keys(a).map(e=>{let s=null==r?void 0:r[e],n=null==d?void 0:d[e];if(null===s)return null;let i=t(s)||t(n);return a[e][i]}),l=r&&Object.entries(r).reduce((e,s)=>{let[r,n]=s;return void 0===n||(e[r]=n),e},{});return i(e,o,null==s?void 0:null===(n=s.compoundVariants)||void 0===n?void 0:n.reduce((e,s)=>{let{class:r,className:n,...t}=s;return Object.entries(t).every(e=>{let[s,r]=e;return Array.isArray(r)?r.includes({...d,...l}[s]):({...d,...l})[s]===r})?[...e,r,n]:e},[]),null==r?void 0:r.class,null==r?void 0:r.className)}},1567:(e,s,r)=>{"use strict";r.d(s,{cn:()=>i});var n=r(3463),t=r(9795);function i(){for(var e=arguments.length,s=Array(e),r=0;r<e;r++)s[r]=arguments[r];return(0,t.QP)((0,n.$)(s))}},3312:(e,s,r)=>{"use strict";r.d(s,{$:()=>l});var n=r(5155),t=r(2115),i=r(2317),a=r(1027),d=r(1567);let o=(0,a.F)("inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring disabled:pointer-events-none disabled:opacity-50 [&_svg]:pointer-events-none [&_svg]:size-4 [&_svg]:shrink-0",{variants:{variant:{default:"bg-primary text-primary-foreground shadow hover:bg-primary/90",destructive:"bg-destructive text-destructive-foreground shadow-sm hover:bg-destructive/90",outline:"border border-input bg-background shadow-sm hover:bg-accent hover:text-accent-foreground",secondary:"bg-secondary text-secondary-foreground shadow-sm hover:bg-secondary/80",ghost:"hover:bg-accent hover:text-accent-foreground",link:"text-primary underline-offset-4 hover:underline"},size:{default:"h-9 px-4 py-2",sm:"h-8 rounded-md px-3 text-xs",lg:"h-10 rounded-md px-8",icon:"h-9 w-9"}},defaultVariants:{variant:"default",size:"default"}}),l=t.forwardRef((e,s)=>{let{className:r,variant:t,size:a,asChild:l=!1,...c}=e,u=l?i.DX:"button";return(0,n.jsx)(u,{className:(0,d.cn)(o({variant:t,size:a,className:r})),ref:s,...c})});l.displayName="Button"},4621:(e,s,r)=>{Promise.resolve().then(r.bind(r,6970))},6970:(e,s,r)=>{"use strict";r.r(s),r.d(s,{default:()=>h,dynamic:()=>v});var n=r(5155),t=r(8173),i=r.n(t),a=r(3312),d=r(2115),o=r(1567);let l=d.forwardRef((e,s)=>{let{className:r,...t}=e;return(0,n.jsx)("div",{ref:s,className:(0,o.cn)("rounded-xl border bg-card text-card-foreground shadow",r),...t})});l.displayName="Card";let c=d.forwardRef((e,s)=>{let{className:r,...t}=e;return(0,n.jsx)("div",{ref:s,className:(0,o.cn)("flex flex-col space-y-1.5 p-6",r),...t})});c.displayName="CardHeader";let u=d.forwardRef((e,s)=>{let{className:r,...t}=e;return(0,n.jsx)("div",{ref:s,className:(0,o.cn)("font-semibold leading-none tracking-tight",r),...t})});u.displayName="CardTitle";let m=d.forwardRef((e,s)=>{let{className:r,...t}=e;return(0,n.jsx)("div",{ref:s,className:(0,o.cn)("text-sm text-muted-foreground",r),...t})});m.displayName="CardDescription";let p=d.forwardRef((e,s)=>{let{className:r,...t}=e;return(0,n.jsx)("div",{ref:s,className:(0,o.cn)("p-6 pt-0",r),...t})});p.displayName="CardContent",d.forwardRef((e,s)=>{let{className:r,...t}=e;return(0,n.jsx)("div",{ref:s,className:(0,o.cn)("flex items-center p-6 pt-0",r),...t})}).displayName="CardFooter";let f=[{id:"1",name:"Food & Beverage",slug:"food-beverage",description:"Restaurants, cafes, bakeries, and more",icon:"\uD83C\uDF54",count:429},{id:"2",name:"Shopping",slug:"shopping",description:"Retail stores, boutiques, and marketplaces",icon:"\uD83D\uDECD️",count:357},{id:"3",name:"Services",slug:"services",description:"Professional and personal services",icon:"\uD83D\uDD27",count:283},{id:"4",name:"Health & Beauty",slug:"health-beauty",description:"Salons, spas, gyms, and medical clinics",icon:"\uD83D\uDC87‍♀️",count:219},{id:"5",name:"Home & Garden",slug:"home-garden",description:"Home improvement, furniture, and gardening",icon:"\uD83C\uDFE1",count:187},{id:"6",name:"Automotive",slug:"automotive",description:"Car dealerships, repair shops, and services",icon:"\uD83D\uDE97",count:156},{id:"7",name:"Education",slug:"education",description:"Schools, tutoring centers, and training institutes",icon:"\uD83D\uDCDA",count:134},{id:"8",name:"Technology",slug:"technology",description:"IT services, electronics stores, and tech companies",icon:"\uD83D\uDCBB",count:128}],v="force-static";function h(){return(0,n.jsx)("div",{className:"container mx-auto py-8 px-4",children:(0,n.jsxs)("div",{className:"max-w-3xl mx-auto",children:[(0,n.jsxs)("div",{className:"text-center mb-8",children:[(0,n.jsx)("h1",{className:"text-3xl font-bold mb-2",children:"Add Your Business"}),(0,n.jsx)("p",{className:"text-muted-foreground",children:"Join the dekat.me business directory and connect with new customers in Malaysia"})]}),(0,n.jsxs)(l,{children:[(0,n.jsxs)(c,{children:[(0,n.jsx)(u,{children:"Business Information"}),(0,n.jsx)(m,{children:"This form is available in the full application but disabled in this demo."})]}),(0,n.jsxs)(p,{className:"space-y-6",children:[(0,n.jsxs)("div",{className:"space-y-4",children:[(0,n.jsx)("h3",{className:"text-lg font-medium border-b pb-2",children:"Business Categories"}),(0,n.jsx)("div",{className:"grid grid-cols-2 md:grid-cols-4 gap-2",children:f.map(e=>(0,n.jsxs)("div",{className:"border rounded p-2 text-center",children:[(0,n.jsx)("div",{className:"text-2xl mb-1",children:e.icon}),(0,n.jsx)("div",{className:"text-sm font-medium",children:e.name})]},e.id))})]}),(0,n.jsxs)("div",{className:"pt-4 text-center",children:[(0,n.jsx)("p",{className:"mb-4 text-muted-foreground",children:"In the full application, you can submit your business details, including contact information, address, and more."}),(0,n.jsx)(a.$,{asChild:!0,children:(0,n.jsx)(i(),{href:"/",children:"Return to Homepage"})})]})]})]})]})})}}},e=>{var s=s=>e(e.s=s);e.O(0,[345,441,587,358],()=>s(4621)),_N_E=e.O()}]);