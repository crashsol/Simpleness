// translate router.meta.title, be used in breadcrumb sidebar tagsview
// 多语言组件，将路由信息进行转换
export function generateTitle(title) {
  const hasKey = this.$te('route.' + title)
  const translatedTitle = this.$t('route.' + title) // $t :this method from vue-i18n, inject in @/lang/index.js

  if (hasKey) {
    return translatedTitle
  }
  return title
}
